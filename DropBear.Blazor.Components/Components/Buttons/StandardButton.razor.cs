#region

using System.Globalization;
using DropBear.Blazor.Components.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

#endregion

namespace DropBear.Blazor.Components.Components.Buttons;

public partial class StandardButton : ComponentBase
{
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string IconClass { get; set; } = string.Empty;
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }
    [Parameter] public string Type { get; set; } = "button";
    [Parameter] public string Title { get; set; } = string.Empty;
    [Parameter] public ButtonStyle Style { get; set; } = ButtonStyle.Primary;
    [Parameter] public ButtonSize Size { get; set; } = ButtonSize.Medium;
    [Parameter] public bool IsOutline { get; set; }
    [Parameter] public bool IsBlock { get; set; }
    [Parameter] public bool IsLightMode { get; set; }

    private string GetButtonClasses()
    {
        var classes = "standard-button";
        classes += $" standard-button-{Style.ToString().ToLower(CultureInfo.CurrentCulture)}";
        classes += $" standard-button-{Size.ToString().ToLower(CultureInfo.CurrentCulture)}";
        if (IsOutline)
        {
            classes += " standard-button-outline";
        }

        if (IsBlock)
        {
            classes += " standard-button-block";
        }

        if (IsLightMode)
        {
            classes += " light-mode";
        }

        if (Disabled)
        {
            classes += " disabled";
        }

        return classes;
    }

    private async Task OnClickHandler(MouseEventArgs args)
    {
        if (!Disabled)
        {
            await OnClick.InvokeAsync(args);
        }
    }
}
