#region

using DropBear.Blazor.Components.Enums;
using DropBear.Blazor.Components.Services.Snackbar;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

#endregion

namespace DropBear.Blazor.Components.Components.Messages;

public partial class Snackbar : ComponentBase, IAsyncDisposable
{
    private readonly List<SnackbarItem> _activeSnackbars = [];
    private IJSObjectReference? _module;
    private ElementReference _snackbarContainerRef;

    #region IAsyncDisposable Members

    public async ValueTask DisposeAsync()
    {
        if (_module is not null)
        {
            await _module.DisposeAsync();
        }
    }

    #endregion

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _module = await JsRuntime.InvokeAsync<IJSObjectReference>("import",
                "./_content/DropBear.Blazor.Components/snackbarInterop.js");
            await InitializeJavaScript();
        }
    }

    private async Task InitializeJavaScript()
    {
        await JsRuntime.InvokeVoidAsync("eval", @"
                window.snackbarInterop = {
                    animateProgress: function(snackbarId, duration) {
                        const snackbar = document.querySelector(`[data-snackbar-id='${snackbarId}']`);
                        if (snackbar) {
                            const progress = snackbar.querySelector('.snackbar-progress');
                            if (progress) {
                                progress.style.transition = `width ${duration}ms linear`;
                                progress.style.width = '0%';
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
            ");
    }

    public async Task AddSnackbar(SnackbarItem snackbar)
    {
        _activeSnackbars.Add(snackbar);
        StateHasChanged();

        await JsRuntime.InvokeVoidAsync("snackbarInterop.animateProgress", snackbar.Id, snackbar.Duration);

        await Task.Delay(snackbar.Duration);
        await RemoveSnackbar(snackbar.Id);
    }

    private async Task RemoveSnackbar(Guid id)
    {
        var snackbar = _activeSnackbars.Find(s => s.Id == id);
        if (snackbar is not null)
        {
            _activeSnackbars.Remove(snackbar);
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
