#region

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

#endregion

namespace DropBear.Blazor.Components.Components.Buttons;

public partial class StandardButton : ComponentBase
{
    [Parameter] public string Variant { get; set; } = "primary";
    [Parameter] public string Size { get; set; } = "default";
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public string IconClass { get; set; } = "";
    [Parameter] public string Title { get; set; } = "";
    [Parameter] public string Type { get; set; } = "button";
    [Parameter] public bool IsBlock { get; set; }

    [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }

    private string ButtonClasses =>
        $"btn standard-button-{Variant} {SizeClass} {(IsBlock ? "btn-block" : "")} {(IsIconOnly ? "btn-icon-only" : "")}".Trim();

    private string SizeClass => Size switch
    {
        "sm" => "btn-sm",
        "lg" => "btn-lg",
        _ => ""
    };

    private bool IsIconOnly => !string.IsNullOrEmpty(IconClass) && ChildContent is null;

    private async Task OnClickHandler(MouseEventArgs args)
    {
        if (OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync(args);
        }
    }
}
