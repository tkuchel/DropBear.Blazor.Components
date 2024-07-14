#region

using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Components.Cards;

public partial class StandardCard : ComponentBase
{
    [Parameter] public string Header { get; set; } = "Card Header";
    [Parameter] public string IconClass { get; set; } = "fas fa-question-circle";
#pragma warning disable CA1056
    [Parameter] public string? ImageUrl { get; set; }
#pragma warning restore CA1056
    [Parameter] public string ImageAlt { get; set; } = "Card Image";
    [Parameter] public string BodyTitle { get; set; } = "";
    [Parameter] public RenderFragment BodyContent { get; set; } = default!;
    [Parameter] public bool ShowFooter { get; set; } = true;
    [Parameter] public EventCallback OnFooterButtonClick { get; set; }
    [Parameter] public string FooterButtonText { get; set; } = "Button";
    [Parameter] public string AdditionalClasses { get; set; } = "";
    [Parameter] public bool IsCompact { get; set; }
}
