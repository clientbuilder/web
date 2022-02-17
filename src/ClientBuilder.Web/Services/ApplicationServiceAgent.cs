using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ClientBuilder.Web.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ClientBuilder.Web.Services;

public class ApplicationServiceAgent : IApplicationServiceAgent
{
    private readonly HttpClient httpClient;

    private readonly JsonSerializerSettings serializerSettings = new ()
    {
        ContractResolver = new DefaultContractResolver()
        {
            NamingStrategy = new CamelCaseNamingStrategy(),
        },
        Formatting = Formatting.Indented,
    };

    public ApplicationServiceAgent(IApplicationStore applicationStore)
    {
        var currentApplication = applicationStore.SelectedApplication;

        this.httpClient = new HttpClient
        {
            BaseAddress = new Uri(currentApplication.Url),
        };

        applicationStore.ApplicationChanged += (_, application) =>
        {
            this.httpClient.BaseAddress = new Uri(application.Url);
        };
    }

    public async Task<IEnumerable<ScaffoldModule>> FetchScaffoldModulesAsync()
    {
        try
        {
            var modulesJson = await this.httpClient.GetStringAsync(this.GetRelativeApiEndpoint("modules"));
            return JsonConvert.DeserializeObject<IEnumerable<ScaffoldModule>>(modulesJson, this.serializerSettings);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new List<ScaffoldModule>();
        }
    }

    private string GetRelativeApiEndpoint(string route)
    {
        return $"/_cb/api/scaffold/{route}";
    }
}