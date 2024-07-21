#region

using System.Globalization;
using DropBear.Blazor.Components.Enums;
using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Components.Components.Cards;

public partial class PromptCard : ComponentBase
{
    [Parameter] public string Title { get; set; } = "Prompt";
    [Parameter] public string Content { get; set; } = "Are you sure you want to continue?";
    [Parameter] public PromptType Type { get; set; } = PromptType.Information;
    [Parameter] public bool IsVisible { get; set; }
    [Parameter] public bool IsLightMode { get; set; }
    [Parameter] public bool IsSubtle { get; set; }
    [Parameter] public bool ShowConfirmButton { get; set; } = true;
    [Parameter] public bool ShowCancelButton { get; set; } = true;
    [Parameter] public string ConfirmText { get; set; } = "Confirm";
    [Parameter] public string CancelText { get; set; } = "Cancel";
    [Parameter] public string ConfirmIcon { get; set; } = "fas fa-check";
    [Parameter] public string CancelIcon { get; set; } = "fas fa-times";
    [Parameter] public EventCallback OnConfirmClick { get; set; }
    [Parameter] public EventCallback OnCancelClick { get; set; }

    private string GetOverlayClasses()
    {
        var classes = "prompt-card-overlay";
        if (IsVisible)
        {
            classes += " visible";
        }

        if (IsLightMode)
        {
            classes += " light-mode";
        }

        return classes;
    }

    private string GetCardClasses()
    {
        var classes = "prompt-card";
        classes += $" {Type.ToString().ToLower(CultureInfo.CurrentCulture)}";
        if (IsLightMode)
        {
            classes += " light-mode";
        }

        if (IsSubtle)
        {
            classes += " subtle";
        }

        return classes;
    }

    private string GetIconClass()
    {
        return Type switch
        {
            PromptType.Information => "fas fa-info-circle",
            PromptType.Warning => "fas fa-exclamation-triangle",
            PromptType.Error => "fas fa-exclamation-circle",
            PromptType.Success => "fas fa-check-circle",
            _ => "fas fa-info-circle"
        };
    }

    public void ShowCard()
    {
        IsVisible = true;
        StateHasChanged();
    }

    public void HideCard()
    {
        IsVisible = false;
        StateHasChanged();
    }
}
