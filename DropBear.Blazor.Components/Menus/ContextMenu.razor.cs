#region

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

#endregion

namespace DropBear.Blazor.Components.Menus;

public abstract partial class ContextMenu : ComponentBase
{
    private double _left;
    private ElementReference _menuElement;
    private double _top;
    [Parameter] public RenderFragment ChildContent { get; set; } = default!;
#pragma warning disable CA1002
    [Parameter] public List<ContextMenuItem?> MenuItems { get; set; } = [];
#pragma warning restore CA1002
    [Parameter] public EventCallback<ContextMenuItem?> OnMenuItemClick { get; set; }
    [Parameter] public string BackgroundColor { get; set; } = "#2b2d31";
    [Parameter] public string TextColor { get; set; } = "#a4b1cd";
    [Parameter] public string HighlightColor { get; set; } = "#4ebafd";
    [Parameter] public bool IsSubmenu { get; set; }

    private bool IsVisible { get; set; }
    private ContextMenuItem? SelectedItem { get; set; }
    private string AnimationClass => IsVisible ? "menu-enter" : "menu-leave";
    private string CustomStyle => $"background-color: {BackgroundColor}; color: {TextColor};";

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && !IsSubmenu)
        {
            await JS.InvokeVoidAsync("contextMenuInterop.initialize", _menuElement, DotNetObjectReference.Create(this));
        }
    }

    [JSInvokable]
    public async Task Show(double left, double top)
    {
        _left = left;
        _top = top;
        IsVisible = true;
        StateHasChanged();
        await Task.Delay(10); // Allow time for the menu to render
        await JS.InvokeVoidAsync("contextMenuInterop.adjustPosition", _menuElement);
        await JS.InvokeVoidAsync("contextMenuInterop.focusMenu", _menuElement);
    }

    public async Task ShowAtPosition(double left, double top)
    {
        await Show(left, top);
    }

    [JSInvokable]
    public void Hide()
    {
        IsVisible = false;
        SelectedItem = null;
        StateHasChanged();
    }

    private async Task OnItemClick(ContextMenuItem? item)
    {
        if (item is not null && !item.HasSubmenu)
        {
            await OnMenuItemClick.InvokeAsync(item);
            Hide();
        }
    }

    private async Task HandleKeyDown(KeyboardEventArgs e)
    {
        switch (e.Key)
        {
            case "ArrowDown":
                SelectNextItem();
                break;
            case "ArrowUp":
                SelectPreviousItem();
                break;
            case "Enter":
                if (SelectedItem != null)
                {
                    await OnItemClick(SelectedItem);
                }

                break;
            case "Escape":
                Hide();
                break;
        }
    }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    private async Task HandleItemKeyDown(KeyboardEventArgs e, ContextMenuItem? item)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        if (item is not null && e.Key is "ArrowRight" && item.HasSubmenu)
        {
            // Open submenu
            if (item.SubmenuItems is not null)
            {
                SelectedItem = item.SubmenuItems.FirstOrDefault();
            }
        }
        else if (e.Key is "ArrowLeft" && IsSubmenu)
        {
            // Close submenu
            Hide();
        }
    }

    private void SelectNextItem()
    {
        var index = MenuItems.IndexOf(SelectedItem);
        index = (index + 1) % MenuItems.Count;
        SelectedItem = MenuItems[index];
    }

    private void SelectPreviousItem()
    {
        var index = MenuItems.IndexOf(SelectedItem);
        index = (index - 1 + MenuItems.Count) % MenuItems.Count;
        SelectedItem = MenuItems[index];
    }
}
