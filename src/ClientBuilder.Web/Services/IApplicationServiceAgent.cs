using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClientBuilder.Web.Models;

namespace ClientBuilder.Web.Services;

public interface IApplicationServiceAgent
{
    event EventHandler AvailabilityChanged;

    event EventHandler EnteredPendingState;

    event EventHandler GenerationCompleted;

    bool Available { get; }

    bool Pending { get; set; }

    Task SyncAvailabilityAsync();

    Task<IEnumerable<ScaffoldModule>> FetchScaffoldModulesAsync();

    Task<GenerationResult> GenerateAsync();

    Task<GenerationResult> GenerateAsync(string moduleId);

    Task<GenerationResult> GenerateByInstanceTypeAsync(InstanceType instanceType);

    Task<GenerationResult> GenerateByClientIdAsync(string clientId);
}