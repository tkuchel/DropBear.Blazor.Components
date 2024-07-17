#region

using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Components.Data;

public class ColumnDefinition<TItem>
{
    public string Title { get; set; } = string.Empty;
    public string Field { get; set; } = string.Empty;
    public string IconClass { get; set; } = string.Empty;
    public Func<TItem, object?> ValueGetter { get; set; } = _ => null;
    public RenderFragment<TItem>? CellTemplate { get; set; }
}
