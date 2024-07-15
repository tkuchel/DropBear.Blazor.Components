#region

using DropBear.Blazor.Components.Messages;

#endregion

namespace DropBear.Blazor.Components.Services;

public sealed class AlertService
{
    private readonly List<AlertMessage> _alerts = [];

    public EventHandler<AlertEventArgs>? OnChange { get; set; } = default!;

    public IReadOnlyCollection<AlertMessage> Alerts => _alerts.AsReadOnly();

    public void RegisterAlert(AlertMessage alert)
    {
        if (_alerts.Contains(alert))
        {
            return;
        }

        _alerts.Add(alert);
        OnChange?.Invoke(this, new AlertEventArgs());
    }

    public void RemoveAlert(AlertMessage alert)
    {
        if (_alerts.Remove(alert))
        {
            OnChange?.Invoke(this, new AlertEventArgs());
        }
    }

    public void Clear()
    {
        _alerts.Clear();
        OnChange?.Invoke(this, new AlertEventArgs());
    }
}
