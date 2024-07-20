#region

using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Components.Components.Cards;

public partial class StandardCard : ComponentBase
{
    [Parameter] public bool IsLightMode { get; set; }
    [Parameter] public bool IsCompact { get; set; }
    [Parameter] public string? ImgSrc { get; set; }
    [Parameter] public string? ImgAlt { get; set; }
    [Parameter] public string? Icon { get; set; }
    [Parameter] public string Title { get; set; } = "Card Title";
    [Parameter] public string? BodyTitle { get; set; }
    [Parameter] public RenderFragment? BodyContent { get; set; }
    [Parameter] public RenderFragment? FooterContent { get; set; }
}
