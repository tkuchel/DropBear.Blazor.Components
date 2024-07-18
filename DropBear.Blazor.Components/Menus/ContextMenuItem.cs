namespace DropBear.Blazor.Components.Menus;

public class ContextMenuItem
{
    public string Text { get; set; } = "";
    public string IconClass { get; set; } = "";
    public bool IsSeparator { get; set; }
#pragma warning disable CA1002
    public List<ContextMenuItem>? SubmenuItems { get; set; } = [];
#pragma warning restore CA1002
    public bool HasSubmenu => SubmenuItems is not null && SubmenuItems.Count is not 0;
}
