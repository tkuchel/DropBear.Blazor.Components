namespace DropBear.Blazor.Components.Menus;

public abstract class ContextMenuItem
{
    public string Text { get; } = "";
    public string IconClass { get; } = "";
    public bool IsSeparator { get; }
#pragma warning disable CA1002
    public List<ContextMenuItem>? SubmenuItems { get; } = [];
#pragma warning restore CA1002
    public bool HasSubmenu => SubmenuItems is not null && SubmenuItems.Count is not 0;
}
