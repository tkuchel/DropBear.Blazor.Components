﻿#region

using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Components.Loaders;

public partial class LongWaitProgresBar : ComponentBase
{
    [Parameter] public string Title { get; set; } = "Long Wait Progress Bar";
    [Parameter] public string ProcessingText { get; set; } = "Processing your request";
    [Parameter] public string IconClass { get; set; } = "fas fa-tasks";
    [Parameter] public bool ShowTitle { get; set; } = true;
    [Parameter] public bool ShowProcessingText { get; set; } = true;
    [Parameter] public bool ShowPercentage { get; set; } = true;
    [Parameter] public bool IsCompact { get; set; }
    [Parameter] public int Progress { get; set; }

    private string CardStyle => IsCompact ? "padding: 0.75rem;" : "";
}
