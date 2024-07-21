#region

using System.Collections.Concurrent;
using DropBear.Blazor.Components.Enums;
using DropBear.Blazor.Components.Services.Snackbar;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

#endregion

namespace DropBear.Blazor.Components.Components.Messages;

public partial class Snackbar : ComponentBase, IAsyncDisposable
{
    private const string JavaScriptFunctions = """
                                                   window.snackbarInterop = {
                                                       animateProgress: function(snackbarId, duration) {
                                                           const snackbar = document.querySelector(`[data-snackbar-id='${snackbarId}']`);
                                                           if (snackbar) {
                                                               const progress = snackbar.querySelector('.snackbar-progress');
                                                               if (progress) {
                                                                   progress.style.transition = `width ${duration}ms linear`;
                                                                   progress.style.width = '0%';
                                                                   progress.setAttribute('aria-valuenow', '100');
                                                                   setTimeout(() => progress.setAttribute('aria-valuenow', '0'), duration);
                                                               }
                                                           }
                                                       },
                                                       removeSnackbar: function(snackbarId) {
                                                           const snackbar = document.querySelector(`[data-snackbar-id='${snackbarId}']`);
                                                           if (snackbar) {
                                                               snackbar.style.animation = 'slideOutAndShrink 0.3s ease-out forwards';
                                                               setTimeout(() => snackbar.remove(), 300);
                                                           }
                                                       }
                                                   };
                                               """;

    private readonly ConcurrentDictionary<Guid, SnackbarItem> _activeSnackbars = new();
    private bool _jsInitialized;

    [Parameter] public SnackbarPosition Position { get; set; } = SnackbarPosition.BottomLeft;
    [Parameter] public bool IsLightMode { get; set; }

    #region IAsyncDisposable Members

    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
        // Cleanup code if needed
    }

    #endregion

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await InitializeJavaScript();
        }
    }

    private async Task InitializeJavaScript()
    {
        await JsRuntime.InvokeVoidAsync("eval", JavaScriptFunctions);
        _jsInitialized = true;
    }

    public async Task AddSnackbar(SnackbarItem snackbar)
    {
        _activeSnackbars[snackbar.Id] = snackbar;
        StateHasChanged();

        if (!_jsInitialized)
        {
            await InitializeJavaScript();
        }

        await JsRuntime.InvokeVoidAsync("snackbarInterop.animateProgress", snackbar.Id, snackbar.Duration);

        await Task.Delay(snackbar.Duration);
        await RemoveSnackbar(snackbar.Id);
    }

    private async Task RemoveSnackbar(Guid id)
    {
        if (_activeSnackbars.TryRemove(id, out _))
        {
            StateHasChanged();
            await JsRuntime.InvokeVoidAsync("snackbarInterop.removeSnackbar", id);
        }
    }

    private static string GetIconClass(SnackbarType type)
    {
        return type switch
        {
            SnackbarType.Info => "fas fa-info-circle",
            SnackbarType.Success => "fas fa-check-circle",
            SnackbarType.Warning => "fas fa-exclamation-triangle",
            SnackbarType.Error => "fas fa-times-circle",
            _ => "fas fa-info-circle"
        };
    }
}
