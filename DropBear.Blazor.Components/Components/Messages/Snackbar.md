# Snackbar Component

The Snackbar component in DropBear.Blazor.Components provides a flexible and customizable way to display brief messages
to users in your Blazor applications.

## Features

- Multiple snackbar types: Info, Success, Warning, Error
- Customizable duration
- Optional action button
- Automatic stacking and limiting of visible snackbars
- Progress indicator
- Smooth enter and exit animations
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

3. Register the `SnackbarService` in your `Program.cs` (or `Startup.cs` for older Blazor versions):

```csharp
builder.Services.AddScoped<SnackbarService>();
```

4. Add the JavaScript interop file reference to your `_Host.cshtml` (for Server-side Blazor) or `index.html` (for
   WebAssembly):

```html
<script src="_content/DropBear.Blazor.Components/snackbarInterop.js"></script>
```

## Usage

1. Add the Snackbar component to your `MainLayout.razor` or any page where you want to use snackbars:

```html
@inject SnackbarService SnackbarService

<Snackbar @ref="SnackbarComponent" />

@code {
    private Snackbar SnackbarComponent;

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            SnackbarService.SetSnackbarComponent(SnackbarComponent);
        }
    }
}
```

2. To show a snackbar from any component, inject the `SnackbarService` and use the `ShowSnackbar` method:

```csharp
@inject SnackbarService SnackbarService

// ...

await SnackbarService.ShowSnackbar("Operation successful!", SnackbarType.Success);
```

## SnackbarService Methods

| Method                                                                                             | Description                                                                          |
|----------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------------------|
| ShowSnackbar(string message, SnackbarType type, int duration = 3000, SnackbarAction action = null) | Displays a snackbar with the specified message, type, duration, and optional action. |

## SnackbarType Enum

- `Info`
- `Success`
- `Warning`
- `Error`

## SnackbarAction Class

| Property | Type   | Description                                      |
|----------|--------|--------------------------------------------------|
| Text     | string | The text to display on the action button         |
| Handler  | Action | The action to perform when the button is clicked |

## Examples

### Basic Usage

```csharp
await SnackbarService.ShowSnackbar("File uploaded successfully!", SnackbarType.Success);
```

### With Custom Duration

```csharp
await SnackbarService.ShowSnackbar("Processing your request...", SnackbarType.Info, 5000);
```

### With Action Button

```csharp
await SnackbarService.ShowSnackbar("Changes saved.", SnackbarType.Success, 3000, new SnackbarAction
{
    Text = "Undo",
    Handler = () => UndoChanges()
});
```

## Styling

The Snackbar component uses CSS variables for easy customization. You can override these variables in your own CSS to
change the appearance of the snackbars:

```css
:root {
  --clr-surface: #2b2d31;
  --clr-text: #ffffff;
  --clr-info: #4ebafd;
  --clr-success: #7cd651;
  --clr-warning: #ffd14d;
  --clr-error: #ff5757;
  --border-radius: 8px;
}
```

## Customization

To further customize the appearance or behavior of the Snackbar component, you can modify
the `Snackbar.razor`, `Snackbar.razor.css`, and `snackbarInterop.js` files in your project.

## Accessibility

The Snackbar component is designed with accessibility in mind:

- It uses appropriate ARIA attributes for screen readers.
- The snackbars automatically disappear after a set duration, preventing permanent obstruction of the UI.
- Action buttons are fully keyboard accessible.

## Browser Support

This component is compatible with all modern browsers, including the latest versions of Chrome, Firefox, Safari, and
Edge.

## Conclusion

The Snackbar component provides a user-friendly way to display brief, non-intrusive messages in your Blazor application.
By leveraging the `SnackbarService`, you can easily show snackbars from any component, enhancing the user experience
with timely and relevant feedback.
