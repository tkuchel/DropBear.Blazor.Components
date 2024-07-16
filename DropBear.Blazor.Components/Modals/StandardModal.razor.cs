#region

using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Components.Modals;

public partial class StandardModal : ComponentBase
{
    [Parameter] public string Title { get; set; } = "Modal Title";
    [Parameter] public RenderFragment? BodyContent { get; set; }
    [Parameter] public string ConfirmText { get; set; } = "Confirm";
    [Parameter] public string CancelText { get; set; } = "Cancel";
    [Parameter] public bool ShowCancelButton { get; set; } = true;
    [Parameter] public bool CloseOnBackdropClick { get; set; } = true;
    [Parameter] public EventCallback OnConfirm { get; set; }
    [Parameter] public EventCallback OnCancel { get; set; }
    [Parameter] public EventCallback OnClose { get; set; }

    private bool IsVisible { get; set; }

    public void Show()
    {
        IsVisible = true;
    }

    public void Hide()
    {
        IsVisible = false;
    }

    private async Task CloseClick()
    {
        if (CloseOnBackdropClick)
        {
            await OnClose.InvokeAsync();
        }
    }
}
