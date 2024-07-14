# Loading Components

DropBear.Blazor.Components provides two loading components to indicate progress in your Blazor applications: ShortWaitSpinner and LongWaitProgressBar.

## ShortWaitSpinner

The ShortWaitSpinner component is designed for short waiting periods and displays an animated spinner.

### Usage

```html
<ShortWaitSpinner Title="Loading Data" 
                  LoadingText="Please wait" 
                  SpinnerColor="#ff0000" 
                  SpinnerSize="75" />
```

### Properties

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| Title | string | "Short Wait Spinner" | The title displayed above the spinner |
| LoadingText | string | "Loading" | The text displayed below the spinner |
| IconClass | string | "fas fa-clock" | The Font Awesome icon class for the title |
| ShowTitle | bool | true | Whether to show the title |
| ShowLoadingText | bool | true | Whether to show the loading text |
| IsCompact | bool | false | Use a more compact layout |
| SpinnerColor | string | "#4ebafd" | The color of the spinner |
| SpinnerSize | int | 50 | The size of the spinner in pixels |

## LongWaitProgressBar

The LongWaitProgressBar component is suitable for longer waiting periods and displays a progress bar with a percentage.

### Usage

```html
<LongWaitProgressBar Title="Uploading Files" 
                     ProcessingText="Please wait while we upload your files" 
                     Progress="65" 
                     ShowPercentage="true" />
```

### Properties

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| Title | string | "Long Wait Progress Bar" | The title displayed above the progress bar |
| ProcessingText | string | "Processing your request" | The text displayed below the progress bar |
| IconClass | string | "fas fa-tasks" | The Font Awesome icon class for the title |
| ShowTitle | bool | true | Whether to show the title |
| ShowProcessingText | bool | true | Whether to show the processing text |
| ShowPercentage | bool | true | Whether to show the percentage in the progress bar |
| IsCompact | bool | false | Use a more compact layout |
| Progress | int | 0 | The current progress percentage (0-100) |

## Styling

Both components use CSS variables for easy customization. You can override these variables in your own CSS to change the colors and styles of the components.

Example:

```css
:root {
    --clr-primary: #4ebafd;
    --clr-secondary: #8552f8;
    --clr-background: #1e1f22;
    --clr-text: #a4b1cd;
}
```

For more advanced styling, you can create your own CSS classes and apply them to the components using the `class` attribute.

## Accessibility

Both components are designed with accessibility in mind. They use appropriate ARIA attributes and ensure that the progress information is available to screen readers.

## Browser Support

These components are compatible with all modern browsers, including the latest versions of Chrome, Firefox, Safari, and Edge.
