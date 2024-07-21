#region

using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Components.Components.Loaders;

public partial class LongWaitProgressBar : ComponentBase
{
#pragma warning disable CA1823
    private const double GoldenRatio = 1.618;
#pragma warning restore CA1823
    private const double InverseGoldenRatio = 0.618;
    [Parameter] public string Title { get; set; } = "Long Wait Progress Bar";
    [Parameter] public string ProcessingText { get; set; } = "Processing your request";
    [Parameter] public string IconClass { get; set; } = "fas fa-tasks";
    [Parameter] public bool ShowTitle { get; set; } = true;
    [Parameter] public bool ShowProcessingText { get; set; } = true;
    [Parameter] public bool ShowPercentage { get; set; } = true;
    [Parameter] public bool IsCompact { get; set; }
    [Parameter] public int Progress { get; set; }
    [Parameter] public EventCallback OnCancel { get; set; }
    [Parameter] public bool ShowCancelButton { get; set; } = true;
    [Parameter] public string CancelButtonText { get; set; } = "Cancel";
    [Parameter] public bool IsLightMode { get; set; }

    private string CardStyle => IsCompact ? $"padding: {0.75 * InverseGoldenRatio}rem;" : "";

    private async Task HandleCancelClick()
    {
        await OnCancel.InvokeAsync();
    }
}
