#region

using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Components.Components.Loaders;

public partial class ShortWaitSpinner : ComponentBase
{
#pragma warning disable CA1823
    private const double GoldenRatio = 1.618;
#pragma warning restore CA1823
    private const double InverseGoldenRatio = 0.618;
    [Parameter] public string Title { get; set; } = "Short Wait Spinner";
    [Parameter] public string LoadingText { get; set; } = "Loading";
    [Parameter] public string IconClass { get; set; } = "fas fa-clock";
    [Parameter] public bool ShowTitle { get; set; } = true;
    [Parameter] public bool ShowLoadingText { get; set; } = true;
    [Parameter] public bool IsCompact { get; set; }
    [Parameter] public int SpinnerSize { get; set; } = 50;
    [Parameter] public bool IsLightMode { get; set; }

    private string SpinnerStyle => $"width: {SpinnerSize}px; height: {SpinnerSize}px;";
    private string CardStyle => IsCompact ? $"padding: {0.75 * InverseGoldenRatio}rem;" : "";
}
