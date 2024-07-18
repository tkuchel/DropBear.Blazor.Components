#region

using DropBear.Blazor.Components.Helpers.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

#endregion

namespace DropBear.Blazor.Components.Components.Menus;

public partial class StandardContextMenu : ComponentBase
{
    private double _left;
    private double _top;

    private ElementReference _menuElement;
    private bool IsVisible { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }
#pragma warning disable CA1002
    [Parameter] public List<ContextMenuItem> MenuItems { get; set; } = [];
#pragma warning restore CA1002
    [Parameter] public EventCallback<ContextMenuItem> OnMenuItemClick { get; set; }
    [Parameter] public string BackgroundColor { get; set; } = "#2b2d31";
    [Parameter] public string TextColor { get; set; } = "#a4b1cd";
    [Parameter] public string HighlightColor { get; set; } = "#4ebafd";

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await InitializeJavaScript();
            await JsRuntime.InvokeVoidAsync("addGlobalClickListener", DotNetObjectReference.Create(this));
        }
    }

    private async Task InitializeJavaScript()
    {
        await JsRuntime.InvokeVoidAsync("eval", @"
                window.getWindowSize = function() {
                    return {
                        width: window.innerWidth,
                        height: window.innerHeight
                    };
                };

                window.getBoundingClientRect = function(element) {
                    const rect = element.getBoundingClientRect();
                    return {
                        left: rect.left,
                        top: rect.top,
                        right: rect.right,
                        bottom: rect.bottom,
                        width: rect.width,
                        height: rect.height
                    };
                };

                window.addGlobalClickListener = function(dotnetHelper) {
                    document.addEventListener('click', function(e) {
                        dotnetHelper.invokeMethodAsync('HandleGlobalClick');
                    });
                };
            ");
    }

    public async Task Show(double left, double top)
    {
        _left = left;
        _top = top;
        IsVisible = true;
        StateHasChanged();
        await Task.Delay(10); // Allow time for the menu to render
        await AdjustPosition();
    }

    public void Hide()
    {
        IsVisible = false;
        StateHasChanged();
    }

    private async Task OnItemClick(ContextMenuItem item)
    {
        if (!item.HasSubmenu)
        {
            await OnMenuItemClick.InvokeAsync(item);
            Hide();
        }
    }

    private async Task AdjustPosition()
    {
        var windowSize = await JsRuntime.InvokeAsync<WindowSize>("getWindowSize");
        var menuRect = await JsRuntime.InvokeAsync<BoundingClientRect>("getBoundingClientRect", _menuElement);

        if (menuRect.Right > windowSize.Width)
        {
            _left = windowSize.Width - menuRect.Width;
        }

        if (menuRect.Bottom > windowSize.Height)
        {
            _top = windowSize.Height - menuRect.Height;
        }

        StateHasChanged();
    }

    [JSInvokable]
    public void HandleGlobalClick()
    {
        Hide();
    }
}
