#region

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;

#endregion

namespace DropBear.Blazor.Components.Components.Forms;

public partial class StandardTextEdit : InputBase<string>
{
    [Parameter] public string Id { get; set; } = Guid.NewGuid().ToString();
    [Parameter] public string Label { get; set; } = string.Empty;
    [Parameter] public string Placeholder { get; set; } = string.Empty;
    [Parameter] public string Type { get; set; } = "text";
    [Parameter] public bool IsDisabled { get; set; }
    [Parameter] public bool IsError { get; set; }
    [Parameter] public bool IsValid { get; set; }
    [Parameter] public string ErrorMessage { get; set; } = string.Empty;
    [Parameter] public string ValidMessage { get; set; } = string.Empty;
    [Parameter] public bool ShowIcon { get; set; }
    [Parameter] public string IconClass { get; set; } = "fas fa-user";
    [Parameter] public bool ShowCharacterCount { get; set; }
    [Parameter] public int? MaxLength { get; set; }
    [Parameter] public bool IsLightMode { get; set; }
    [Parameter] public EventCallback<FocusEventArgs> OnFocus { get; set; }
    [Parameter] public EventCallback<FocusEventArgs> OnBlur { get; set; }
    [Parameter] public EventCallback OnIconClick { get; set; }

    private string GetWrapperClasses()
    {
        var classes = new List<string> { "standard-textbox-wrapper" };
        if (IsLightMode)
        {
            classes.Add("theme-light");
        }
        else
        {
            classes.Add("theme-dark");
        }

        return string.Join(' ', classes);
    }

    private string GetInputClasses()
    {
        var classes = new List<string> { "standard-textbox" };
        if (IsError)
        {
            classes.Add("error");
        }

        if (IsValid)
        {
            classes.Add("valid");
        }

        return string.Join(' ', classes);
    }

    private async Task OnFocusHandler(FocusEventArgs args)
    {
        await OnFocus.InvokeAsync(args);
    }

    private async Task OnBlurHandler(FocusEventArgs args)
    {
        await OnBlur.InvokeAsync(args);
    }

    private async Task OnIconClickHandler()
    {
        await OnIconClick.InvokeAsync();
    }

    protected override bool TryParseValueFromString(string? value, out string result, out string validationErrorMessage)
    {
        result = value ?? string.Empty;
        validationErrorMessage = string.Empty;
        return true;
    }
}
