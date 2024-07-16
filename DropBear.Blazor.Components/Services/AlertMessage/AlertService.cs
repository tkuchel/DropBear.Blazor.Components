namespace DropBear.Blazor.Components.Services.AlertMessage;

public sealed class AlertService
{
    private readonly List<Messages.AlertMessage> _alerts = [];

    public EventHandler<AlertEventArgs>? OnChange { get; set; } = default!;

    public IReadOnlyCollection<Messages.AlertMessage> Alerts => _alerts.AsReadOnly();

    public void RegisterAlert(Messages.AlertMessage alert)
    {
        if (_alerts.Contains(alert))
        {
            return;
        }

        _alerts.Add(alert);
        OnChange?.Invoke(this, new AlertEventArgs());
    }

    public void RemoveAlert(Messages.AlertMessage alert)
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
