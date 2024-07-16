# AlertMessage Component and AlertService

The AlertMessage component and AlertService in DropBear.Blazor.Components provide a flexible and powerful system for
displaying and managing alerts in your Blazor applications.

## Features

- Multiple alert types: Info, Success, Warning, Danger
- Customizable severity levels: Low, Normal, High
- Auto-dismiss functionality
- Support for custom icons
- Centralized alert management through AlertService
- Dismissible and non-dismissible alerts
- Animated appearance and dismissal

## Installation

1. Install the NuGet package in your Blazor project:

```
dotnet add package DropBear.Blazor.Components
```

2. Add the following line to your `_Imports.razor` file:

```csharp
@using DropBear.Blazor.Components
```

3. Register the AlertService in your `Startup.cs` (for Blazor Server) or `Program.cs` (for Blazor WebAssembly):

```csharp
builder.Services.AddScoped<AlertService>();
```

## Usage

### Basic Usage

```html
@inject AlertService AlertService

<AlertMessage Type="AlertMessage.AlertType.Info" Title="Information">
    This is an informational message.
</AlertMessage>
```

### Using AlertService

```html
@inject AlertService AlertService

<div class="alert-container">
    @foreach (var alert in AlertService.Alerts)
    {
        <AlertMessage @key="alert" Type="@alert.Type" Severity="@alert.Severity" Title="@alert.Title" 
                      IsDismissible="@alert.IsDismissible" OnDismiss="@alert.OnDismiss" 
                      CustomIconClass="@alert.CustomIconClass" AutoDismissAfter="@alert.AutoDismissAfter">
            @alert.ChildContent
        </AlertMessage>
    }
</div>

<button @onclick="AddAlert">Add Alert</button>

@code {
    private void AddAlert()
    {
        AlertService.RegisterAlert(new AlertMessage
        {
            Type = AlertMessage.AlertType.Success,
            Severity = AlertMessage.AlertSeverity.Normal,
            Title = "Success",
            ChildContent = builder => builder.AddContent(0, "Operation completed successfully."),
            AutoDismissAfter = 5000 // Auto-dismiss after 5 seconds
        });
    }
}
```

## AlertMessage Properties

| Property         | Type           | Default | Description                                                                         |
|------------------|----------------|---------|-------------------------------------------------------------------------------------|
| Type             | AlertType      | Info    | The type of alert (Info, Success, Warning, Danger)                                  |
| Severity         | AlertSeverity  | Normal  | The severity level of the alert (Low, Normal, High)                                 |
| Title            | string         | null    | The title of the alert                                                              |
| ChildContent     | RenderFragment | null    | The main content of the alert                                                       |
| IsDismissible    | bool           | true    | Whether the alert can be dismissed by the user                                      |
| OnDismiss        | EventCallback  | null    | Event callback when the alert is dismissed                                          |
| CustomIconClass  | string         | null    | Custom icon class (e.g., "fas fa-bell")                                             |
| AutoDismissAfter | int            | 0       | Time in milliseconds after which the alert auto-dismisses (0 means no auto-dismiss) |

## AlertService Methods

| Method                            | Description                                  |
|-----------------------------------|----------------------------------------------|
| RegisterAlert(AlertMessage alert) | Adds a new alert to the service              |
| RemoveAlert(AlertMessage alert)   | Removes the specified alert from the service |
| Clear()                           | Removes all alerts from the service          |

## Styling

The AlertMessage component comes with built-in styles. You can further customize the appearance by overriding the CSS
variables or adding your own classes.

```css
:root {
    --clr-info: #4ebafd;
    --clr-success: #7cd651;
    --clr-warning: #ffd14d;
    --clr-danger: #ff5757;
    --clr-text: #ffffff;
    --clr-text-muted: #a4b1cd;
}
```

To style the alert container, you can add the following CSS:

```css
.alert-container {
    position: fixed;
    top: 20px;
    right: 20px;
    width: 300px;
    z-index: 1000;
}

.alert-container > :not(:last-child) {
    margin-bottom: 10px;
}
```

## Advanced Usage

### Custom Icons

```html
<AlertMessage Type="AlertMessage.AlertType.Info" CustomIconClass="fas fa-bell" Title="Notification">
    You have a new message.
</AlertMessage>
```

### High Severity Alert

```html
<AlertMessage Type="AlertMessage.AlertType.Danger" Severity="AlertMessage.AlertSeverity.High" Title="Critical Error">
    The system encountered a critical error. Please contact support immediately.
</AlertMessage>
```

### Non-dismissible Alert

```html
<AlertMessage Type="AlertMessage.AlertType.Warning" IsDismissible="false" Title="Ongoing Maintenance">
    System maintenance is in progress. Some features may be unavailable.
</AlertMessage>
```

### Auto-dismissing Alert

```html
<AlertMessage Type="AlertMessage.AlertType.Success" AutoDismissAfter="3000" Title="Saved">
    Your changes have been saved successfully.
</AlertMessage>
```

## Best Practices

1. Use appropriate alert types and severity levels to convey the right level of importance.
2. Keep alert messages concise and clear.
3. Use auto-dismiss for less critical notifications to avoid cluttering the UI.
4. Consider using the AlertService for managing multiple alerts, especially in larger applications.
5. Implement a consistent alert style across your application for a better user experience.

## Accessibility

The AlertMessage component is designed with accessibility in mind. It uses appropriate ARIA attributes and ensures that
alerts can be dismissed using the keyboard.

## Browser Support

This component is compatible with all modern browsers, including the latest versions of Chrome, Firefox, Safari, and
Edge.

## Conclusion

The AlertMessage component and AlertService provide a comprehensive solution for managing and displaying alerts in your
Blazor applications. By leveraging these tools, you can create informative, visually appealing, and user-friendly
notifications that enhance the overall user experience of your application.
