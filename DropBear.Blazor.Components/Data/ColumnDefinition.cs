namespace DropBear.Blazor.Components.Data;

public class ColumnDefinition<T>
{
    public string Title { get; set; } = "";
    public string Field { get; set; } = "";
    public string IconClass { get; set; } = "";
    public Func<T, object?> ValueGetter { get; set; } = item => item;
}
