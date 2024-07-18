#region

using System.Globalization;
using DropBear.Blazor.Components.Enums;
using DropBear.Blazor.Components.Services.AlertMessage;
using Microsoft.AspNetCore.Components;
using Timer = System.Timers.Timer;

#endregion

namespace DropBear.Blazor.Components.Components.Messages;

public partial class AlertMessage : ComponentBase, IDisposable
{
    private Timer? _autoDismissTimer;
    [Parameter] public AlertType Type { get; set; } = AlertType.Info;
    [Parameter] public AlertSeverity Severity { get; set; } = AlertSeverity.Normal;
    [Parameter] public string Title { get; set; } = string.Empty;
    [Parameter] public RenderFragment ChildContent { get; set; } = default!;
    [Parameter] public bool IsDismissible { get; set; } = true;
    [Parameter] public EventCallback OnDismiss { get; set; }
    [Parameter] public string CustomIconClass { get; set; } = string.Empty;
    [Parameter] public int AutoDismissAfter { get; set; } // in milliseconds, 0 means no auto-dismiss

    [Inject] private AlertService? AlertService { get; set; }

    private bool IsVisible { get; set; } = true;

    private string AlertClasses =>
        $"alert alert-{Type.ToString().ToLower(CultureInfo.CurrentCulture)} alert-{Severity.ToString().ToLower(CultureInfo.CurrentCulture)} {(IsDismissible ? "" : "alert-dismissible")} {(IsVisible ? "" : "alert-hidden")}"
            .Trim();

    private string IconClass => !string.IsNullOrEmpty(CustomIconClass)
        ? CustomIconClass
        : Type switch
        {
            AlertType.Success => "fas fa-check-circle",
            AlertType.Warning => "fas fa-exclamation-triangle",
            AlertType.Danger => "fas fa-times-circle",
            _ => "fas fa-info-circle"
        };

    #region IDisposable Members

    public void Dispose()
    {
        _autoDismissTimer?.Dispose();
        AlertService?.RemoveAlert(this);
    }

    #endregion

    protected override void OnInitialized()
    {
        AlertService?.RegisterAlert(this);

        if (AutoDismissAfter <= 0)
        {
            return;
        }

        _autoDismissTimer = new Timer(AutoDismissAfter);
        _autoDismissTimer.Elapsed += async (sender, e) => await DismissAlert();
        _autoDismissTimer.Start();
    }

    private async Task DismissAlert()
    {
        IsVisible = false;
        await OnDismiss.InvokeAsync();
        AlertService?.RemoveAlert(this);
        _autoDismissTimer?.Stop();
    }
}
