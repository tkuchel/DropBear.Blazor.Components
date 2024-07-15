#region

using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Components.Menus;

public partial class ContextSubmenu : ComponentBase
{
#pragma warning disable CA1002
    [Parameter] public List<ContextMenuItem>? MenuItems { get; set; } = [];
#pragma warning restore CA1002
    [Parameter] public bool IsVisible { get; set; } = true;
    [Parameter] public string SubmenuStyle { get; set; } = "";
    [Parameter] public EventCallback<ContextMenuItem> OnSubmenuItemClick { get; set; }

    private ContextMenuItem? SelectedItem { get; set; }

    private async Task OnItemClick(ContextMenuItem item)
    {
        if (!item.HasSubmenu)
        {
            await OnSubmenuItemClick.InvokeAsync(item)!;
        }
    }
}
