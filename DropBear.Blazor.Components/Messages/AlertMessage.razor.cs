#region

using System.Globalization;
using DropBear.Blazor.Components.Enums;
using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Components.Messages;

public partial class AlertMessage : ComponentBase
{
    [Parameter] public AlertType Type { get; set; } = AlertType.Info;
    [Parameter] public string Title { get; set; } = "";
    [Parameter] public RenderFragment ChildContent { get; set; } = default!;
    [Parameter] public bool IsDismissible { get; set; } = true;
    [Parameter] public EventCallback OnDismiss { get; set; }

    private bool IsVisible { get; set; } = true;

    private string AlertClasses =>
        $"alert alert-{Type.ToString().ToLower(CultureInfo.CurrentCulture)} {(IsDismissible ? "" : "alert-dismissible")} {(IsVisible ? "" : "alert-hidden")}"
            .Trim();

    private string IconClass => Type switch
    {
        AlertType.Success => "fas fa-check-circle",
        AlertType.Warning => "fas fa-exclamation-triangle",
        AlertType.Danger => "fas fa-times-circle",
        _ => "fas fa-info-circle"
    };

    private async Task DismissAlert()
    {
        IsVisible = false;
        await OnDismiss.InvokeAsync();
    }
}
