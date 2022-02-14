using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClientBuilder.Web.Helpers.Bootstrap;
using Microsoft.JSInterop;

namespace ClientBuilder.Web.Services;

public class BootstrapService : IBootstrapService
{
    private readonly IJSRuntime jsRuntime;

    public BootstrapService(IJSRuntime jsRuntime)
    {
        this.jsRuntime = jsRuntime;
        this.Toasts = new List<Toast>();
    }

    public event EventHandler HasStateChange;

    public IList<Toast> Toasts { get; private set; }

    public async Task ShowModalAsync(string modalId)
    {
        await this.jsRuntime.InvokeVoidAsync("showModal", modalId);
    }

    public async Task CloseModalAsync(string modalId)
    {
        await this.jsRuntime.InvokeVoidAsync("closeModal", modalId);
    }

    public void ShowToast(string message, string variant)
    {
        var toast = new Toast { Message = message, Variant = variant };
        this.Toasts.Add(toast);

        this.TriggerStateChangeEvent();
    }

    public void RemoveToast(string id)
    {
        var toastForRemoval = this.Toasts.FirstOrDefault(x => x.Id == id);
        if (toastForRemoval != null)
        {
            this.Toasts.Remove(toastForRemoval);
            this.TriggerStateChangeEvent();
        }
    }

    private void TriggerStateChangeEvent()
    {
        this.HasStateChange?.Invoke(this, EventArgs.Empty);
    }
}