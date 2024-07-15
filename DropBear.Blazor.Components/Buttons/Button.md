# Button Component

The Button component in DropBear.Blazor.Components provides a flexible and customizable button for your Blazor applications.

## Features

- Multiple button variants (primary, secondary, outline, danger)
- Size options (small, default, large)
- Icon support
- Block-level button option
- Disabled state
- Customizable styling through CSS variables

## Usage

```html
<Button Variant="primary" Size="default" OnClick="@HandleClick">
    Click me!
</Button>

<Button Variant="secondary" IconClass="fas fa-edit">
    Edit
</Button>

<Button Variant="outline" Size="lg" IsBlock="true">
    Large Block Button
</Button>

<Button Variant="danger" IconClass="fas fa-trash-alt" />
```

## Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| Variant | string | "primary" | The style variant of the button (primary, secondary, outline, danger) |
| Size | string | "default" | The size of the button (sm, default, lg) |
| Disabled | bool | false | Whether the button is disabled |
| IconClass | string | "" | The class for the icon (e.g., "fas fa-edit") |
| Title | string | "" | The title attribute for the button |
| Type | string | "button" | The type attribute for the button (button, submit, reset) |
| IsBlock | bool | false | Whether the button should be a block-level element |
| OnClick | EventCallback<MouseEventArgs> | - | Event callback for when the button is clicked |
| ChildContent | RenderFragment | null | The content to be displayed inside the button |

## Styling

The Button component uses CSS variables for easy customization. You can override these variables in your own CSS to change the appearance of the buttons:

```css
:root {
  --clr-primary: #4ebafd;
  --clr-secondary: #8552f8;
  --clr-danger: #ff5757;
  --clr-surface: #2b2d31;
  --clr-text: #ffffff;
  --border-radius: 8px;
  --transition: all 0.3s ease;
}
```

## CSS Classes

The Button component generates CSS classes based on the provided parameters. The main classes are:

- `.btn`: Applied to all buttons
- `.btn-primary`, `.btn-secondary`, `.btn-outline`, `.btn-danger`: Applied based on the `Variant` parameter
- `.btn-sm`, `.btn-lg`: Applied based on the `Size` parameter
- `.btn-block`: Applied when `IsBlock` is true
- `.btn-icon-only`: Applied when only an icon is provided (no child content)

## Examples

### Primary Button with Icon

```html
<Button Variant="primary" IconClass="fas fa-save" OnClick="@SaveData">
    Save
</Button>
```

### Disabled Secondary Button

```html
<Button Variant="secondary" Disabled="true">
    Disabled Button
</Button>
```

### Large Block-Level Outline Button

```html
<Button Variant="outline" Size="lg" IsBlock="true">
    Full Width Button
</Button>
```

### Icon-Only Danger Button

```html
<Button Variant="danger" IconClass="fas fa-trash-alt" Title="Delete" />
```

## Accessibility

The Button component is designed with accessibility in mind:

- It uses the native `<button>` element, ensuring proper keyboard navigation and screen reader support.
- The `Title` parameter can be used to provide additional context for icon-only buttons.
- Disabled state is properly conveyed to assistive technologies.

## Best Practices

1. Use clear and concise text for button labels.
2. Provide an `IconClass` and `Title` for icon-only buttons to ensure they are understandable.
3. Use the appropriate `Variant` to convey the button's purpose or importance.
4. Consider using `IsBlock="true"` for important actions on mobile devices to increase tap target size.

## Browser Support

This component is compatible with all modern browsers, including the latest versions of Chrome, Firefox, Safari, and Edge.

## Conclusion

The Button component provides a versatile and customizable way to create buttons in your Blazor application. By leveraging its various parameters and styling options, you can create consistent and attractive buttons that enhance your user interface.
