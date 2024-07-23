#region

using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Components.Components.Data;

// ReSharper disable once ClassNeverInstantiated.Global
public class DataGridColumn<TItem>
{
    internal string PropertyName { get; set; } = string.Empty;
    internal string Title { get; set; } = string.Empty;
    public Func<TItem, object>? PropertySelector { get; set; }
    public RenderFragment<TItem>? Template { get; set; }
    public bool Sortable { get; set; }
    public bool Filterable { get; set; }
    internal double Width { get; set; } = 100; // Default width
}
