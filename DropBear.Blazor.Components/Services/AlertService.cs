#region

using DropBear.Blazor.Components.Messages;

#endregion

namespace DropBear.Blazor.Components.Services;

public class AlertService
{
    private readonly List<AlertMessage> _alerts = new();

    public IReadOnlyCollection<AlertMessage> Alerts => _alerts.AsReadOnly();
    public event Action OnChange;

    public void RegisterAlert(AlertMessage alert)
    {
        if (!_alerts.Contains(alert))
        {
            _alerts.Add(alert);
            OnChange?.Invoke();
        }
    }

    public void RemoveAlert(AlertMessage alert)
    {
        if (_alerts.Remove(alert))
        {
            OnChange?.Invoke();
        }
    }

    public void Clear()
    {
        _alerts.Clear();
        OnChange?.Invoke();
    }
}
