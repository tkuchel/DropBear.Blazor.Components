#region

using DropBear.Blazor.Components.Enums;

#endregion

namespace DropBear.Blazor.Components.Services.Snackbar;

public class SnackbarItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Message { get; set; } = string.Empty;
    public SnackbarType Type { get; set; } = SnackbarType.Info;
    public int Duration { get; set; } = 3000;
    public SnackbarAction? Action { get; set; }
}
