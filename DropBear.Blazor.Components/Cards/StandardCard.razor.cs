#region

using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Components.Cards;

public partial class StandardCard : ComponentBase
{
    [Parameter] public string Header { get; set; }

    [Parameter] public string IconClass { get; set; }

    [Parameter] public string ImageUrl { get; set; }

    [Parameter] public string ImageAlt { get; set; }

    [Parameter] public string BodyTitle { get; set; }

    [Parameter] public RenderFragment BodyContent { get; set; }

    [Parameter] public bool ShowFooter { get; set; }

    [Parameter] public EventCallback OnFooterButtonClick { get; set; }

    [Parameter] public string AdditionalClasses { get; set; }
}
