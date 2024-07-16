#region

using DropBear.Blazor.Components.Enums;
using DropBear.Blazor.Components.Services.Snackbar;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

#endregion

namespace DropBear.Blazor.Components.Messages;

public partial class Snackbar : ComponentBase
{
    private readonly List<SnackbarItem> _activeSnackbars = [];
    private ElementReference _snackbarContainerRef;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JSRuntime.InvokeVoidAsync("snackbarInterop.initialize", _snackbarContainerRef);
        }
    }

    public async Task AddSnackbar(SnackbarItem snackbar)
    {
        _activeSnackbars.Add(snackbar);
        StateHasChanged();

        await JSRuntime.InvokeVoidAsync("snackbarInterop.animateProgress", snackbar.Id, snackbar.Duration);

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
            await JSRuntime.InvokeVoidAsync("snackbarInterop.removeSnackbar", id);
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
