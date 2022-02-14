using System;
using System.Net.Http;
using Blazored.LocalStorage;
using ClientBuilder.Web;
using ClientBuilder.Web.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<IApplicationStore, ApplicationStore>();

builder.Services.AddScoped<IBootstrapService, BootstrapService>();

var host = builder.Build();
var applicationStore = host.Services.GetRequiredService<IApplicationStore>();

await applicationStore.LoadStoreAsync();

await host.RunAsync();