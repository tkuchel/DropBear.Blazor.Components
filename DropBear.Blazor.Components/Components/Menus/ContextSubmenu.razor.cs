#region

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

#endregion

namespace DropBear.Blazor.Components.Components.Menus;

public partial class ContextSubmenu : ComponentBase
{
#pragma warning disable CA1002
    [Parameter] public List<ContextMenuItem>? MenuItems { get; set; } = [];
#pragma warning restore CA1002
    [Parameter] public bool IsVisible { get; set; } = true;
    [Parameter] public string SubmenuStyle { get; set; } = "";
    [Parameter] public EventCallback<ContextMenuItem> OnSubmenuItemClick { get; set; }
    [Parameter] public bool IsLightMode { get; set; }

    private ContextMenuItem? SelectedItem { get; set; }

    private async Task OnItemClick(ContextMenuItem item)
    {
        if (!item.HasSubmenu)
        {
            await OnSubmenuItemClick.InvokeAsync(item);
        }
    }

    private async Task OnKeyDown(KeyboardEventArgs e, ContextMenuItem item)
    {
        switch (e.Key)
        {
            case "Enter":
            case " ":
                await OnItemClick(item);
                break;
            case "Escape":
                // Close submenu
                break;
        }
    }
}
