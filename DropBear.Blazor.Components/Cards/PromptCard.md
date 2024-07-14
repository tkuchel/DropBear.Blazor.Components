# PromptCard Component

The PromptCard component is a versatile and customizable Blazor component for displaying various types of prompts, alerts, and confirmations in your application.

## Features

- Multiple prompt types: Information, Warning, Error, Success, and Confirmation
- Customizable content, buttons, and icons
- Subtle variation for less intrusive prompts
- Light and dark mode support
- Animated appearance
- Responsive design

## Installation

1. Install the NuGet package in your Blazor project:

```
dotnet add package DropBear.Blazor.Components
```

2. Add the following line to your `_Imports.razor` file:

```csharp
@using DropBear.Blazor.Components
```

3. Add the following line to your `_Host.cshtml` (for Server-side Blazor) or `index.html` (for WebAssembly):

```html
<link href="_content/DropBear.Blazor.Components/Styles/global-styles.css" rel="stylesheet" />
```

## Usage

Here's a basic example of how to use the PromptCard component:

```html
<PromptCard @ref="promptCard"
            Title="Confirm Action"
            Content="Are you sure you want to proceed?"
            CardType="PromptCard.PromptType.Confirmation"
            IsVisible="@isVisible"
            OnConfirmClick="HandleConfirm"
            OnCancelClick="HandleCancel" />
```

In your `@code` section:

```csharp
private PromptCard promptCard;
private bool isVisible;

private void ShowPrompt()
{
    isVisible = true;
    promptCard.ShowCard();
}

private void HandleConfirm()
{
    promptCard.HideCard();
    // Add your confirm logic here
}

private void HandleCancel()
{
    promptCard.HideCard();
    // Add your cancel logic here
}
```

## Properties

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| Title | string | "Prompt" | The title of the prompt card |
| Content | string | "This is a prompt message." | The main content of the prompt card |
| CardType | PromptType | PromptType.Information | The type of the prompt (Information, Warning, Error, Success, Confirmation) |
| IsVisible | bool | false | Controls the visibility of the prompt card |
| ShowConfirmButton | bool | true | Controls the visibility of the confirm button |
| ShowCancelButton | bool | true | Controls the visibility of the cancel button |
| ConfirmText | string | "Confirm" | The text for the confirm button |
| CancelText | string | "Cancel" | The text for the cancel button |
| ConfirmIcon | string | "fas fa-check" | The icon class for the confirm button |
| CancelIcon | string | "fas fa-times" | The icon class for the cancel button |
| IsSubtle | bool | false | Creates a more subdued version of the card |
| IsLightMode | bool | false | Switches the card to light mode |

## Events

| Event | Description |
|-------|-------------|
| OnConfirmClick | Triggered when the confirm button is clicked |
| OnCancelClick | Triggered when the cancel button is clicked |

## Methods

| Method | Description |
|--------|-------------|
| ShowCard() | Makes the card visible |
| HideCard() | Hides the card |

## Customization Examples

### Information Prompt

```html
<PromptCard Title="Information"
            Content="This is an informational message."
            CardType="PromptCard.PromptType.Information"
            IsVisible="@isVisible"
            ShowCancelButton="false"
            ConfirmText="OK" />
```

### Warning Prompt (Subtle)

```html
<PromptCard Title="Warning"
            Content="This action may have consequences."
            CardType="PromptCard.PromptType.Warning"
            IsVisible="@isVisible"
            IsSubtle="true" />
```

### Error Prompt

```html
<PromptCard Title="Error"
            Content="An error occurred while processing your request."
            CardType="PromptCard.PromptType.Error"
            IsVisible="@isVisible"
            ConfirmText="Retry"
            CancelText="Close" />
```

### Success Prompt (Light Mode)

```html
<PromptCard Title="Success"
            Content="Your changes have been saved successfully."
            CardType="PromptCard.PromptType.Success"
            IsVisible="@isVisible"
            IsLightMode="true"
            ShowCancelButton="false"
            ConfirmText="OK" />
```

### Confirmation Prompt

```html
<PromptCard Title="Confirm Deletion"
            Content="Are you sure you want to delete this item? This action cannot be undone."
            CardType="PromptCard.PromptType.Confirmation"
            IsVisible="@isVisible"
            ConfirmText="Yes, Delete"
            CancelText="No, Keep It"
            ConfirmIcon="fas fa-trash"
            CancelIcon="fas fa-undo" />
```

## Styling

The PromptCard component uses CSS variables for easy customization. You can override these variables in your own CSS to change the colors and styles of the cards.

Example:

```css
:root {
    --var-clr-malibu: #61afff;
    --var-clr-mustard: #ffd14d;
    --var-clr-persimmon: #ff5757;
    --var-clr-pastel-green: #7cd651;
    --var-clr-confirmation: #9b59b6;
}
```

For more advanced styling, you can create your own CSS classes and apply them to the PromptCard using the `class` attribute.

## Accessibility

The PromptCard component is designed with accessibility in mind. It uses semantic HTML and ARIA attributes to ensure that it's usable by people with disabilities. The component traps focus when visible, allowing users to navigate through the card using the keyboard.

## Browser Support

The PromptCard component is compatible with all modern browsers, including the latest versions of Chrome, Firefox, Safari, and Edge.

## Contributing

If you find any issues or have suggestions for improvements, please file an issue on our GitHub repository or submit a pull request.

## License

This component is released under the MIT License. See the LICENSE file for details.
