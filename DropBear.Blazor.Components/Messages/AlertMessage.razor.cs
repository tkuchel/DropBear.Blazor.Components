#region

using DropBear.Blazor.Components.Enums;
using DropBear.Blazor.Components.Services;
using Microsoft.AspNetCore.Components;
using Timer = System.Timers.Timer;

#endregion

namespace DropBear.Blazor.Components.Messages;

public partial class AlertMessage : ComponentBase
{
    private Timer AutoDismissTimer;
    [Parameter] public AlertType Type { get; set; } = AlertType.Info;
    [Parameter] public AlertSeverity Severity { get; set; } = AlertSeverity.Normal;
    [Parameter] public string Title { get; set; }
    [Parameter] public RenderFragment ChildContent { get; set; }
    [Parameter] public bool IsDismissible { get; set; } = true;
    [Parameter] public EventCallback OnDismiss { get; set; }
    [Parameter] public string CustomIconClass { get; set; }
    [Parameter] public int AutoDismissAfter { get; set; } // in milliseconds, 0 means no auto-dismiss

    [Inject] private AlertService AlertService { get; set; }

    private bool IsVisible { get; set; } = true;

    private string AlertClasses =>
        $"alert alert-{Type.ToString().ToLower()} alert-{Severity.ToString().ToLower()} {(IsDismissible ? "" : "alert-dismissible")} {(IsVisible ? "" : "alert-hidden")}"
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

    protected override void OnInitialized()
    {
        AlertService.RegisterAlert(this);
        if (AutoDismissAfter > 0)
        {
            AutoDismissTimer = new Timer(AutoDismissAfter);
            AutoDismissTimer.Elapsed += async (sender, e) => await DismissAlert();
            AutoDismissTimer.Start();
        }
    }

    private async Task DismissAlert()
    {
        IsVisible = false;
        await OnDismiss.InvokeAsync();
        AlertService.RemoveAlert(this);
        AutoDismissTimer?.Stop();
    }

    public void Dispose()
    {
        AutoDismissTimer?.Dispose();
        AlertService.RemoveAlert(this);
    }
}
