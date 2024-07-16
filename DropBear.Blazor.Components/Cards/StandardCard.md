# StandardCard Component

The StandardCard component is a flexible and reusable Blazor component for displaying various types of content in a card
format.

## Features

- Multiple card types: Default, with Icon, with Image, and Compact
- Customizable content, headers, and footers
- Light and dark mode support
- Animated appearance
- Responsive design

## Installation

1. Install the NuGet package in your Blazor project:

```bash
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

Here's a basic example of how to use the StandardCard component:

```html
<StandardCard Header="Default Card"
              BodyContent="This is a standard card that can wrap any content. It's versatile and matches our existing design language."
              ShowFooter="true"
              OnFooterButtonClick="HandleFooterButtonClick" />
```

In your `@code` section:

```csharp
private void HandleFooterButtonClick()
{
    // Add your footer button click logic here
}
```

## Properties

| Property            | Type           | Default | Description                                           |
|---------------------|----------------|---------|-------------------------------------------------------|
| Header              | string         | null    | The header text of the card                           |
| IconClass           | string         | null    | The icon class for an optional icon in the header     |
| ImageUrl            | string         | null    | The URL for an optional image at the top of the card  |
| ImageAlt            | string         | null    | The alt text for the image                            |
| BodyTitle           | string         | null    | The title text within the body of the card            |
| BodyContent         | RenderFragment | null    | The main content of the card                          |
| ShowFooter          | bool           | false   | Controls the visibility of the footer section         |
| OnFooterButtonClick | EventCallback  | null    | The event triggered when the footer button is clicked |
| AdditionalClasses   | string         | null    | Additional CSS classes for customization              |

## Customization Examples

### Default Card

```html
<StandardCard Header="Default Card"
              BodyContent="This is a standard card that can wrap any content. It's versatile and matches our existing design language."
              ShowFooter="true"
              OnFooterButtonClick="HandleFooterButtonClick" />
```

### Card with Icon

```html
<StandardCard Header="Card with Icon"
              IconClass="fas fa-star"
              BodyContent="This card demonstrates how we can include an icon in the header for added visual interest." />
```

### Card with Image

```html
<StandardCard ImageUrl="https://via.placeholder.com/400x200"
              ImageAlt="Placeholder"
              BodyTitle="Card with Image"
              BodyContent="This card shows how we can include an image at the top of the card." />
```

### Compact Card

```html
<StandardCard BodyTitle="Compact Card"
              BodyContent="A more compact version of our card, without separate header and footer sections."
              AdditionalClasses="compact" />
```

## Styling

The StandardCard component uses CSS variables for easy customization. You can override these variables in your own CSS
to change the colors and styles of the cards.

Example:

```css
:root {
    --clr-background: #1e1f22;
    --clr-card-bg: #2b2d31;
    --clr-text: #a4b1cd;
    --clr-heading: #ffffff;
    --clr-primary: #4ebafd;
    --clr-secondary: #8552f8;
    --border-radius: 12px;
    --spacing-xs: 0.25rem;
    --spacing-sm: 0.5rem;
    --spacing-md: 0.75rem;
    --spacing-lg: 1rem;
}
```

For more advanced styling, you can create your own CSS classes and apply them to the StandardCard using
the `AdditionalClasses` parameter.

## Accessibility

The StandardCard component is designed with accessibility in mind. It uses semantic HTML and ARIA attributes to ensure
that it's usable by people with disabilities.

## Browser Support

The StandardCard component is compatible with all modern browsers, including the latest versions of Chrome, Firefox,
Safari, and Edge.

## Contributing

If you find any issues or have suggestions for improvements, please file an issue on our GitHub repository or submit a
pull request.

## License

This component is released under the MIT License. See the LICENSE file for details.
