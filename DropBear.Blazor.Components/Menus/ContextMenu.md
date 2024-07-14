# ContextMenu Component

The ContextMenu component in DropBear.Blazor.Components provides a customizable right-click menu for your Blazor applications.

## Features

- Customizable menu items with icons and submenus
- Keyboard navigation
- Automatic positioning within viewport
- Animations for smooth appearance and disappearance
- Custom styling options
- Accessibility support

## Usage

```html
<ContextMenu @ref="ContextMenu" 
             OnMenuItemClick="HandleMenuItemClick"
             BackgroundColor="#333333"
             TextColor="#ffffff"
             HighlightColor="#4ebafd">
    <div @oncontextmenu="ShowContextMenu" @oncontextmenu:preventDefault>
        Right-click me!
    </div>
</ContextMenu>

@code {
    private ContextMenu ContextMenu;

    private List<ContextMenu.ContextMenuItem> _menuItems = new List<ContextMenu.ContextMenuItem>
    {
        new ContextMenu.ContextMenuItem { Text = "Edit", IconClass = "fas fa-edit" },
        new ContextMenu.ContextMenuItem { Text = "Copy", IconClass = "fas fa-copy" },
        new ContextMenu.ContextMenuItem { IsSeparator = true },
        new ContextMenu.ContextMenuItem { Text = "Delete", IconClass = "fas fa-trash-alt" },
        new ContextMenu.ContextMenuItem
        {
            Text = "Share",
            IconClass = "fas fa-share",
            SubmenuItems = new List<ContextMenu.ContextMenuItem>
            {
                new ContextMenu.ContextMenuItem { Text = "Facebook", IconClass = "fab fa-facebook" },
                new ContextMenu.ContextMenuItem { Text = "Twitter", IconClass = "fab fa-twitter" }
            }
        }
    };

    protected override void OnInitialized()
    {
        ContextMenu.MenuItems = _menuItems;
    }

    private async Task ShowContextMenu(MouseEventArgs e)
    {
        await ContextMenu.ShowAtPosition(e.ClientX, e.ClientY);
    }

    private void HandleMenuItemClick(ContextMenu.ContextMenuItem item)
    {
        Console.WriteLine($"Clicked: {item.Text}");
        // Handle the menu item click
    }
}
```

## Properties

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| MenuItems | List<ContextMenuItem> | [] | The list of menu items to display |
| OnMenuItemClick | EventCallback<ContextMenuItem> | - | Event callback for when a menu item is clicked |
| BackgroundColor | string | "#2b2d31" | The background color of the menu |
| TextColor | string | "#a4b1cd" | The text color of the menu items |
| HighlightColor | string | "#4ebafd" | The highlight color for selected or hovered items |

## ContextMenuItem Properties

| Property | Type | Description |
|----------|------|-------------|
| Text | string | The text to display for the menu item |
| IconClass | string | The Font Awesome icon class for the item |
| IsSeparator | bool | If true, renders a separator instead of a clickable item |
| SubmenuItems | List<ContextMenuItem> | A list of submenu items |

## Methods

| Method | Description |
|--------|-------------|
| ShowAtPosition(double left, double top) | Programmatically show the menu at the specified coordinates |
| Hide() | Programmatically hide the menu |

## Keyboard Navigation

- Arrow keys: Navigate through menu items
- Enter: Select the current item
- Escape: Close the menu
- Right arrow: Open submenu (if available)
- Left arrow: Close submenu and return to parent menu

## Styling

The component uses CSS variables for easy customization. You can override these variables in your own CSS or use the component properties to change the colors.

Example:

```css
:root {
    --menu-bg: #2b2d31;
    --menu-text: #a4b1cd;
    --menu-highlight: rgba(78, 186, 253, 0.1);
    --menu-highlight-text: #ffffff;
}
```

## Accessibility

The ContextMenu component is designed to be accessible. It can be fully operated using a keyboard and provides appropriate ARIA attributes for screen readers.

## Browser Support

This component is compatible with all modern browsers, including the latest versions of Chrome, Firefox, Safari, and Edge.

## Notes

- Ensure that you're using Font Awesome in your project for the icons to display correctly.
- The component uses JavaScript interop for positioning and event handling, so make sure you have the necessary JavaScript file included in your project.
