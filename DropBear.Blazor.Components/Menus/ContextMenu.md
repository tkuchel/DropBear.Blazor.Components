# ContextMenu Component

The ContextMenu component in DropBear.Blazor.Components provides a customizable right-click menu for your Blazor applications.

## Features

- Customizable menu items with icons and submenus
- Keyboard navigation
- Automatic positioning within viewport
- Animations for smooth appearance and disappearance
- Custom styling options
- Accessibility support
- Nested submenus support

## Usage

```html
<ContextMenu @ref="ContextMenu"
             OnMenuItemClick="HandleMenuItemClick">
    <div @oncontextmenu="ShowContextMenu" @oncontextmenu:preventDefault>
        Right-click me!
    </div>
</ContextMenu>

@code {
    private ContextMenu ContextMenu;

    private List<ContextMenuItem> _menuItems = new List<ContextMenuItem>
    {
        new ContextMenuItem { Text = "Edit", IconClass = "fas fa-edit" },
        new ContextMenuItem { Text = "Copy", IconClass = "fas fa-copy" },
        new ContextMenuItem { IsSeparator = true },
        new ContextMenuItem { Text = "Delete", IconClass = "fas fa-trash-alt" },
        new ContextMenuItem
        {
            Text = "Share",
            IconClass = "fas fa-share",
            SubmenuItems = new List<ContextMenuItem>
            {
                new ContextMenuItem { Text = "Facebook", IconClass = "fab fa-facebook" },
                new ContextMenuItem { Text = "Twitter", IconClass = "fab fa-twitter" }
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

    private void HandleMenuItemClick(ContextMenuItem item)
    {
        Console.WriteLine($"Clicked: {item.Text}");
        // Handle the menu item click
    }
}
```

## Components

The ContextMenu feature consists of two main components:

1. `ContextMenu.razor`: The main component that handles the top-level menu.
2. `ContextSubmenu.razor`: A separate component for handling nested submenus.

## JavaScript Interop

The ContextMenu component uses JavaScript interop for positioning and event handling. The JavaScript file (`contextMenuInterop.js`) should be included in your project, typically in the `wwwroot/js` folder.

```javascript
export class ContextMenuInterop {
    static initialize(element, dotnetHelper) {
        document.addEventListener('contextmenu', function (e) {
            e.preventDefault();
            dotnetHelper.invokeMethodAsync('Show', e.pageX, e.pageY);
        });

        document.addEventListener('click', function (e) {
            if (!element.contains(e.target)) {
                dotnetHelper.invokeMethodAsync('Hide');
            }
        });
    }
    
    static adjustPosition(element) {
        const rect = element.getBoundingClientRect();
        const windowWidth = window.innerWidth;
        const windowHeight = window.innerHeight;

        if (rect.right > windowWidth) {
            element.style.left = (windowWidth - rect.width) + 'px';
        }

        if (rect.bottom > windowHeight) {
            element.style.top = (windowHeight - rect.height) + 'px';
        }
    }

    static focusMenu(element) {
        element.focus();
    }
}

// Make it globally accessible
window.ContextMenuInterop = ContextMenuInterop;
```

Make sure to add a script reference to your `contextMenuInterop.js` file in your `_Host.cshtml` (for Server-side Blazor) or `index.html` (for WebAssembly):

```html
<script src="_content/DropBear.Blazor.Components/js/contextMenuInterop.js"></script>
```

## Properties

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| MenuItems | List<ContextMenuItem> | [] | The list of menu items to display |
| OnMenuItemClick | EventCallback<ContextMenuItem> | - | Event callback for when a menu item is clicked |
| IsVisible | bool | false | Determines if the menu is visible |
| IsSubmenu | bool | false | Indicates if this menu is a submenu |

## ContextMenuItem Properties

| Property | Type | Description |
|----------|------|-------------|
| Text | string | The text to display for the menu item |
| IconClass | string | The Font Awesome icon class for the item |
| IsSeparator | bool | If true, renders a separator instead of a clickable item |
| SubmenuItems | List<ContextMenuItem> | A list of submenu items |
| HasSubmenu | bool | Indicates if this item has a submenu |

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

The component uses CSS variables for easy customization. You can override these variables in your own CSS file:

```css
:root {
    --context-menu-bg: #2b2d31;
    --context-menu-text: #ffffff;
    --context-menu-hover-bg: #3a3d42;
    --context-menu-hover-text: #ffffff;
    --context-menu-disabled-text: #8e9297;
    --context-menu-separator: #40444b;
    --context-menu-icon-color: #b9bbbe;
    --context-menu-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
    --context-menu-border-radius: 4px;
    --context-menu-padding: 8px 0;
    --context-menu-item-padding: 8px 12px;
    --context-menu-transition: all 0.2s ease;
    --context-menu-font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
    --context-menu-font-size: 14px;
    --context-menu-min-width: 180px;
    --context-menu-z-index: 1000;
}
```

## Accessibility

The ContextMenu component is designed to be accessible. It can be fully operated using a keyboard and provides appropriate ARIA attributes for screen readers.

## Browser Support

This component is compatible with all modern browsers, including the latest versions of Chrome, Firefox, Safari, and Edge.

## Notes

- Ensure that you're using Font Awesome in your project for the icons to display correctly.
- The component uses JavaScript interop for positioning and event handling, so make sure you have included the necessary JavaScript file in your project.
- The ContextSubmenu component is used internally to handle nested submenus, providing a seamless experience for multi-level menus.
