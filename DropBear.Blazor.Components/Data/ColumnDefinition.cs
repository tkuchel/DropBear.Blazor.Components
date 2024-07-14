namespace DropBear.Blazor.Components.Data;

public abstract class ColumnDefinition<T>
{
    public string Title { get; set; } = "";
    public string Field { get; set; } = "";
    public Func<T, object?> ValueGetter { get; set; } = item => item;
    public bool Sortable { get; set; } = true;
}
