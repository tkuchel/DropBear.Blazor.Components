#region

using DropBear.Blazor.Components.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

#endregion

namespace DropBear.Blazor.Components.Modals;

public partial class StandardModal : ComponentBase
{
    private ElementReference _modalElement;
    [Parameter] public string Title { get; set; } = "Modal Title";
    [Parameter] public RenderFragment? BodyContent { get; set; }
    [Parameter] public RenderFragment? CustomHeader { get; set; }
    [Parameter] public RenderFragment? CustomFooter { get; set; }
    [Parameter] public string ConfirmText { get; set; } = "Confirm";
    [Parameter] public string CancelText { get; set; } = "Cancel";
    [Parameter] public bool ShowCancelButton { get; set; } = true;
    [Parameter] public bool CloseOnBackdropClick { get; set; } = true;
    [Parameter] public EventCallback OnConfirm { get; set; }
    [Parameter] public EventCallback OnCancel { get; set; }
    [Parameter] public EventCallback OnClose { get; set; }
    [Parameter] public bool LightMode { get; set; } = false;
    [Parameter] public ModalSize Size { get; set; } = ModalSize.Medium;
    [Parameter] public string CustomCssClass { get; set; } = "";
    [Parameter] public ModalTransition Transition { get; set; } = ModalTransition.Fade;
    [Parameter] public bool ScrollableContent { get; set; } = false;

    private bool IsVisible { get; set; }

    // ReSharper disable once MemberCanBePrivate.Global
    public async Task ShowAsync()
    {
        IsVisible = true;
        await Task.Delay(10); // Allow time for the DOM to update
        await JsRuntime.InvokeVoidAsync("standardModal.show", _modalElement);
    }

    public void Show()
    {
        _ = ShowAsync().ConfigureAwait(false);
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public async Task HideAsync()
    {
        await JsRuntime.InvokeVoidAsync("standardModal.hide", _modalElement);
        IsVisible = false;
    }

    public void Hide()
    {
        _ = HideAsync().ConfigureAwait(false);
    }

    private async Task CloseClick()
    {
        if (CloseOnBackdropClick || LightMode)
        {
            await OnClose.InvokeAsync();
            await HideAsync();
        }
    }

    private async Task ConfirmClick()
    {
        await OnConfirm.InvokeAsync();
        if (LightMode)
        {
            await HideAsync();
        }
    }

    private async Task CancelClick()
    {
        await OnCancel.InvokeAsync();
        if (LightMode)
        {
            await HideAsync();
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JsRuntime.InvokeVoidAsync("standardModal.initialize", _modalElement);
        }
    }
}
