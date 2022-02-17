using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using ClientBuilder.Web.Exceptions;
using ClientBuilder.Web.Models;

namespace ClientBuilder.Web.Services;

public class ApplicationStore : IApplicationStore
{
    private const string ApplicationLocalStorageKey = "_cb_local_apps";
    private const string SelectedApplicationLocalStorageKey = "_cb_local_sel_app";

    private readonly ILocalStorageService localStorageService;
    private IReadOnlyList<Application> applications;
    private Guid selectedApplicationId = Guid.Empty;

    public ApplicationStore(ILocalStorageService localStorageService)
    {
        this.localStorageService = localStorageService;

        this.applications = new List<Application>();
    }

    public event EventHandler<Application> ApplicationChanged;

    public IReadOnlyCollection<Application> Applications => this.applications;

    public Guid SelectedApplicationId => this.selectedApplicationId;

    public Application SelectedApplication => this.applications.FirstOrDefault(x => x.Id == this.selectedApplicationId);

    public async Task SaveApplicationAsync(Application application)
    {
        string applicationName = application.Name;
        if (string.IsNullOrWhiteSpace(applicationName))
        {
            throw new ClientBuilderValidationException("Application name is required");
        }

        string applicationUrl = application.Url;
        if (string.IsNullOrWhiteSpace(applicationUrl))
        {
            throw new ClientBuilderValidationException("Application URL is required");
        }

        if (applicationUrl.EndsWith("/"))
        {
            applicationUrl = applicationUrl[..^1];
        }

        var applicationsList = new List<Application>(this.applications);
        var existingApplication = applicationsList
            .FirstOrDefault(x => x.Url.ToLowerInvariant() == applicationUrl || (application.Id != Guid.Empty && x.Id == application.Id));

        if (existingApplication != null)
        {
            existingApplication.Name = applicationName;
            existingApplication.Url = applicationUrl;
        }
        else
        {
            applicationsList.Add(new Application
            {
                Id = Guid.NewGuid(),
                Name = applicationName,
                Url = applicationUrl,
            });
        }

        await this.SaveApplicationsAsync(applicationsList);
    }

    public async Task DeleteApplicationAsync(Guid applicationId)
    {
        var applicationsList = this.applications.ToList();
        var existingApplication = applicationsList.FirstOrDefault(x => x.Id == applicationId);

        if (existingApplication != null)
        {
            applicationsList.Remove(existingApplication);
        }

        await this.SaveApplicationsAsync(applicationsList);
    }

    public async Task SelectApplicationAsync(Guid applicationId)
    {
        if (this.applications.All(x => x.Id != applicationId))
        {
            return;
        }

        this.selectedApplicationId = applicationId;
        await this.localStorageService.SetItemAsync(SelectedApplicationLocalStorageKey, applicationId);
        this.ApplicationChanged?.Invoke(this, this.applications.FirstOrDefault(x => x.Id == applicationId));
    }

    public async Task LoadStoreAsync()
    {
        await this.InitIfEmptyStoreSetAsync<IEnumerable<Application>>(
            ApplicationLocalStorageKey,
            new List<Application>());

        var storedApplications = await this.localStorageService.GetItemAsync<IEnumerable<Application>>(ApplicationLocalStorageKey);
        this.applications = storedApplications.ToList().AsReadOnly();

        await this.InitIfEmptyStoreSetAsync(
            SelectedApplicationLocalStorageKey,
            this.applications.Select(x => x.Id).FirstOrDefault());

        var storedSelectedApplicationId = await this.localStorageService.GetItemAsync<Guid>(SelectedApplicationLocalStorageKey);
        this.selectedApplicationId = storedSelectedApplicationId;
    }

    private async Task SaveApplicationsAsync(IList<Application> applicationsList)
    {
        await this.localStorageService.SetItemAsync(ApplicationLocalStorageKey, applicationsList);
        await this.LoadStoreAsync();
    }

    private async Task InitIfEmptyStoreSetAsync<T>(string key, T defaultValue)
    {
        if (!await this.localStorageService.ContainKeyAsync(key))
        {
            await this.localStorageService.SetItemAsync(key, defaultValue);
        }
    }
}