using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClientBuilder.Web.Helpers.Bootstrap;

namespace ClientBuilder.Web.Services;

public interface IBootstrapService
{
    event EventHandler HasStateChange;

    IList<Toast> Toasts { get; }

    Task ShowModalAsync(string modalId);

    Task CloseModalAsync(string modalId);

    void ShowToast(string message, string variant);

    void RemoveToast(string id);
}