#region

using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Components.Cards.PromptCard;

public partial class PromptCard : ComponentBase
{
    [Parameter] public string Title { get; set; } = "Professional Blazorise Card";

    [Parameter]
    public string Content { get; set; } =
        "This is a professionally styled card using custom CSS with Blazorise classes and Poppins font.";

    [Parameter] public EventCallback OnConfirmClick { get; set; }
    [Parameter] public EventCallback OnCancelClick { get; set; }
}