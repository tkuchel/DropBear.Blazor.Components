#region

using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Components.Components.Cards;

public partial class StandardCard : ComponentBase
{
    [Parameter] public string ImgSrc { get; set; } = "https://via.placeholder.com/150";
    [Parameter] public string ImgAlt { get; set; } = "Placeholder image";
    [Parameter] public string Icon { get; set; } = "fas fa-ellipsis-h";
    [Parameter] public string Title { get; set; } = "Card Title";
    [Parameter] public string BodyTitle { get; set; } = "Card Body Title";
    [Parameter] public RenderFragment? BodyContent { get; set; }
    [Parameter] public RenderFragment? FooterContent { get; set; }
    [Parameter] public bool IsLightMode { get; set; }
    [Parameter] public bool IsCompact { get; set; }
    [Parameter] public bool EnableHover { get; set; } = true;
}
