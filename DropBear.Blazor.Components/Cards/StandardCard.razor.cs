#region

using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Components.Cards;

public partial class StandardCard : ComponentBase
{
    [Parameter] public string Header { get; set; } = "Card Header";

    [Parameter] public string IconClass { get; set; } = "fas fa-question-circle";

    [Parameter] public Uri ImageUrl { get; set; } = new("https://via.placeholder.com/150");

    [Parameter] public string ImageAlt { get; set; } = "Placeholder Image";

    [Parameter] public string BodyTitle { get; set; } = "Card Body Title";

    [Parameter] public RenderFragment BodyContent { get; set; } = default!;

    [Parameter] public bool ShowFooter { get; set; } = true;

    [Parameter] public EventCallback OnFooterButtonClick { get; set; }

    [Parameter] public string FooterButtonText { get; set; } = "Button";

    [Parameter] public string AdditionalClasses { get; set; } = "";
}
