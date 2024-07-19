#region

using DropBear.Blazor.Components.Enums;

#endregion

namespace DropBear.Blazor.Components.Services.Snackbar;

public class SnackbarService
{
    private Components.Messages.Snackbar? _snackbarComponent;

    public void SetSnackbarComponent(Components.Messages.Snackbar snackbarComponent)
    {
        _snackbarComponent = snackbarComponent;
    }

    public async Task ShowSnackbar(string message, SnackbarType type, int duration = 3000,
        SnackbarAction? action = null, SnackbarPosition position = SnackbarPosition.BottomLeft)
    {
        if (_snackbarComponent is null)
        {
            throw new InvalidOperationException("Snackbar component has not been set.");
        }

        var snackbar = new SnackbarItem
        {
            Id = Guid.NewGuid(),
            Message = message,
            Type = type,
            Duration = duration,
            Action = action ?? new SnackbarAction(),
            Position = position
        };

        await _snackbarComponent.AddSnackbar(snackbar).ConfigureAwait(false);
    }
}
