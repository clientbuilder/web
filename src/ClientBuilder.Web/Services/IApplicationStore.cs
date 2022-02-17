using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClientBuilder.Web.Models;

namespace ClientBuilder.Web.Services;

public interface IApplicationStore
{
    event EventHandler<Application> ApplicationChanged;

    IReadOnlyCollection<Application> Applications { get; }

    Guid SelectedApplicationId { get; }

    Application SelectedApplication { get; }

    Task SaveApplicationAsync(Application application);

    Task DeleteApplicationAsync(Guid applicationId);

    Task SelectApplicationAsync(Guid applicationId);

    Task LoadStoreAsync();
}