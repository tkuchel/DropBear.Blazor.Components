#region

using DropBear.Blazor.Components.Enums;

#endregion

namespace DropBear.Blazor.Components.Services.Snackbar;

public class SnackbarService
{
    private Messages.Snackbar? _snackbarComponent;

    public void SetSnackbarComponent(Messages.Snackbar snackbarComponent)
    {
        _snackbarComponent = snackbarComponent;
    }

    public async Task ShowSnackbar(string message, SnackbarType type, int duration = 3000,
        SnackbarAction? action = null)
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
            Action = action ?? new SnackbarAction()
        };

        await _snackbarComponent.AddSnackbar(snackbar)!.ConfigureAwait(false);
    }
}
