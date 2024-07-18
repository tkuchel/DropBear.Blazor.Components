#region

using System.Globalization;
using DropBear.Blazor.Components.Enums;
using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Components.Components.Cards;

public partial class PromptCard : ComponentBase
{
    [Parameter] public string Title { get; set; } = "Prompt";
    [Parameter] public string Content { get; set; } = "This is a prompt message.";
    [Parameter] public PromptType CardType { get; set; } = PromptType.Information;
    [Parameter] public bool IsVisible { get; set; }
    [Parameter] public EventCallback OnConfirmClick { get; set; }
    [Parameter] public EventCallback OnCancelClick { get; set; }
    [Parameter] public bool ShowConfirmButton { get; set; } = true;
    [Parameter] public bool ShowCancelButton { get; set; } = true;
    [Parameter] public string ConfirmText { get; set; } = "Confirm";
    [Parameter] public string CancelText { get; set; } = "Cancel";
    [Parameter] public string ConfirmIcon { get; set; } = "fas fa-check";
    [Parameter] public string CancelIcon { get; set; } = "fas fa-times";
    [Parameter] public bool IsSubtle { get; set; }
    [Parameter] public bool IsLightMode { get; set; }


    private string GetOverlayClasses()
    {
        return $"prompt-card-overlay {(IsVisible ? "visible" : "")} {(IsLightMode ? "light-mode" : "")}";
    }

    private string GetCardClasses()
    {
        return $"prompt-card {CardType.ToString().ToLower(CultureInfo.CurrentCulture)} {(IsSubtle ? "subtle" : "")}";
    }

    private string GetIconClass()
    {
        return CardType switch
        {
            PromptType.Information => "fas fa-info-circle",
            PromptType.Warning => "fas fa-exclamation-triangle",
            PromptType.Error => "fas fa-times-circle",
            PromptType.Success => "fas fa-check-circle",
            PromptType.Confirmation => "fas fa-question-circle",
            _ => "fas fa-question-circle"
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
