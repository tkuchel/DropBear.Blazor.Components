namespace DropBear.Blazor.Components.Services.Snackbar;

public class SnackbarAction
{
    public string Text { get; set; } = string.Empty;
    public Action Handler { get; set; } = () => { };
}
