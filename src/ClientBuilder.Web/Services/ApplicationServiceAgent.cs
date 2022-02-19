using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ClientBuilder.Web.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ClientBuilder.Web.Services;

public class ApplicationServiceAgent : IApplicationServiceAgent
{
    private readonly JsonSerializerSettings serializerSettings = new ()
    {
        ContractResolver = new DefaultContractResolver()
        {
            NamingStrategy = new CamelCaseNamingStrategy(),
        },
        Formatting = Formatting.Indented,
    };

    private HttpClient httpClient;

    public ApplicationServiceAgent(IApplicationStore applicationStore)
    {
        var currentApplication = applicationStore.SelectedApplication;
        this.httpClient = this.CreateHttpClient(currentApplication);

        applicationStore.ApplicationChanged += async (_, application) =>
        {
            this.httpClient = this.CreateHttpClient(application);
            await this.SyncAvailabilityAsync();
        };
    }

    public event EventHandler AvailabilityChanged;

    public event EventHandler EnteredPendingState;

    public event EventHandler GenerationCompleted;

    public bool Available { get; private set; }

    public bool Pending { get; set; }

    public async Task<IEnumerable<ScaffoldModule>> FetchScaffoldModulesAsync()
    {
        try
        {
            if (!this.Available)
            {
                return new List<ScaffoldModule>();
            }

            var modulesJson = await this.httpClient.GetStringAsync(this.GetRelativeApiEndpoint("modules"));
            return JsonConvert.DeserializeObject<IEnumerable<ScaffoldModule>>(modulesJson, this.serializerSettings);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new List<ScaffoldModule>();
        }
    }

    public async Task<GenerationResult> GenerateAsync() =>
        await this.GenerateAsync("generate", new { ModuleId = string.Empty });

    public async Task<GenerationResult> GenerateAsync(string moduleId) =>
        await this.GenerateAsync("generate", new { ModuleId = moduleId });

    public async Task<GenerationResult> GenerateByInstanceTypeAsync(InstanceType instanceType) =>
        await this.GenerateAsync("generate/by-instance", new { InstanceType = instanceType });

    public async Task<GenerationResult> GenerateByClientIdAsync(string clientId) =>
        await this.GenerateAsync("generate/by-client", new { ClientId = clientId });

    public async Task SyncAvailabilityAsync()
    {
        this.Pending = true;
        this.EnteredPendingState?.Invoke(this, EventArgs.Empty);

        try
        {
            var checkResponse =
                await this.httpClient.PostAsync(this.GetRelativeApiEndpoint("check"), new StringContent(string.Empty));
            this.Available = checkResponse.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            this.Available = false;
        }
        finally
        {
            this.Pending = false;
            this.AvailabilityChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private async Task<GenerationResult> GenerateAsync(string route, object requestData)
    {
        try
        {
            var json = JsonConvert.SerializeObject(requestData, this.serializerSettings);
            var requestContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await this.httpClient.PostAsync(this.GetRelativeApiEndpoint(route), requestContent);
            if (!response.IsSuccessStatusCode)
            {
                return GenerationResult.UnsuccessfulResult("An unexpected error occured");
            }

            var responseString = await response.Content.ReadAsStringAsync();

            this.GenerationCompleted?.Invoke(this, EventArgs.Empty);
            return JsonConvert.DeserializeObject<GenerationResult>(responseString, this.serializerSettings);
        }
        catch (Exception ex)
        {
            this.GenerationCompleted?.Invoke(this, EventArgs.Empty);
            return GenerationResult.UnsuccessfulResult(ex.Message);
        }
    }

    private string GetRelativeApiEndpoint(string route)
    {
        return $"/_cb/api/scaffold/{route}";
    }

    private HttpClient CreateHttpClient(Application application)
    {
        if (application != null)
        {
            return new HttpClient
            {
                BaseAddress = new Uri(application.Url),
            };
        }

        return new HttpClient();
    }
}