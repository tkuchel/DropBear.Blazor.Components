#region

using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Components.Loaders;

public partial class ShortWaitSpinner : ComponentBase
{
    [Parameter] public string Title { get; set; } = "Short Wait Spinner";
    [Parameter] public string LoadingText { get; set; } = "Loading";
    [Parameter] public string IconClass { get; set; } = "fas fa-clock";
    [Parameter] public bool ShowTitle { get; set; } = true;
    [Parameter] public bool ShowLoadingText { get; set; } = true;
    [Parameter] public bool IsCompact { get; set; }
    [Parameter] public string SpinnerColor { get; set; } = "#4ebafd";
    [Parameter] public int SpinnerSize { get; set; } = 50;

    private string SpinnerStyle => $"width: {SpinnerSize}px; height: {SpinnerSize}px; border-color: {SpinnerColor};";
    private string CardStyle => IsCompact ? "padding: 0.75rem;" : "";
}
