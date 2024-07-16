# Modal Component

The Modal component in DropBear.Blazor.Components provides a flexible and customizable modal dialog for your Blazor applications.

## Features

- Customizable title and button text
- Flexible body content using RenderFragment
- Optional cancel button
- Backdrop click to close (optional)
- Smooth open and close animations
- Event callbacks for confirm, cancel, and close actions
- Programmatic show/hide methods

## Installation

1. Add the Modal.razor and Modal.razor.css files to your Blazor project's Components folder.
2. Ensure your _Imports.razor file includes the following line:
   ```csharp
   @using YourProjectName.Components
   ```

## Usage

```html
@using DropBear.Blazor.Components

<Modal @ref="modal" 
       Title="Example Modal" 
       OnConfirm="HandleConfirm" 
       OnCancel="HandleCancel" 
       OnClose="HandleClose">
    <BodyContent>
        <p>This is the modal content. You can put any HTML here.</p>
        <input type="text" placeholder="Enter some text" @bind="inputText" />
    </BodyContent>
</Modal>

@code {
    private Modal modal;
    private string inputText = "";

    private void ShowModal()
    {
        modal.Show();
    }

    private void HandleConfirm()
    {
        Console.WriteLine($"Confirmed! Input text: {inputText}");
        modal.Hide();
    }

    private void HandleCancel()
    {
        Console.WriteLine("Cancelled");
        modal.Hide();
    }

    private void HandleClose()
    {
        Console.WriteLine("Closed");
        modal.Hide();
    }
}
```

## Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| Title | string | "Modal Title" | The title displayed in the modal header |
| BodyContent | RenderFragment | null | The content to be displayed in the modal body |
| ConfirmText | string | "Confirm" | The text for the confirm button |
| CancelText | string | "Cancel" | The text for the cancel button |
| ShowCancelButton | bool | true | Whether to show the cancel button |
| CloseOnBackdropClick | bool | true | Whether to close the modal when clicking outside |
| OnConfirm | EventCallback | - | Event triggered when the confirm button is clicked |
| OnCancel | EventCallback | - | Event triggered when the cancel button is clicked |
| OnClose | EventCallback | - | Event triggered when the modal is closed |

## Methods

| Method | Description |
|--------|-------------|
| Show() | Displays the modal |
| Hide() | Hides the modal |

## Styling

The Modal component uses CSS variables for easy customization. You can override these variables in your own CSS to change the appearance of the modal:

```css
:root {
  --clr-overlay: rgba(0, 0, 0, 0.7);
  --clr-surface: #2b2d31;
  --clr-surface-light: #3a3d44;
  --clr-surface-lighter: #454950;
  --clr-primary: #4ebafd;
  --clr-secondary: #8552f8;
  --clr-text: #ffffff;
  --clr-text-muted: #a4b1cd;
  --border-radius: 12px;
  --font-family: 'Poppins', sans-serif;
}
```

## Customization Examples

### Custom Title and Button Text

```html
<Modal @ref="modal" 
       Title="Confirm Action" 
       ConfirmText="Yes, I'm Sure"
       CancelText="No, Go Back">
    <BodyContent>
        <p>Are you sure you want to proceed with this action?</p>
    </BodyContent>
</Modal>
```

### Modal Without Cancel Button

```html
<Modal @ref="modal" 
       Title="Information" 
       ShowCancelButton="false"
       ConfirmText="OK">
    <BodyContent>
        <p>This is an informational message.</p>
    </BodyContent>
</Modal>
```

### Modal with Complex Content

```html
<Modal @ref="modal" Title="User Profile">
    <BodyContent>
        <EditForm Model="@user" OnValidSubmit="HandleValidSubmit">
            <DataAnnotationsValidator />
            <ValidationSummary />

            <div class="form-group">
                <label for="name">Name:</label>
                <InputText id="name" @bind-Value="user.Name" class="form-control" />
            </div>

            <div class="form-group">
                <label for="email">Email:</label>
                <InputText id="email" @bind-Value="user.Email" class="form-control" />
            </div>

            <button type="submit" class="btn btn-primary">Save Changes</button>
        </EditForm>
    </BodyContent>
</Modal>
```

## Accessibility

The Modal component is designed with accessibility in mind:

- It traps focus within the modal when open
- It can be closed using the ESC key
- It uses appropriate ARIA attributes for screen readers

## Browser Support

This component is compatible with all modern browsers, including the latest versions of Chrome, Firefox, Safari, and Edge.

## Conclusion

The Modal component provides a flexible and customizable way to display modal dialogs in your Blazor application. By leveraging the `BodyContent` RenderFragment, you can include any content within the modal, from simple messages to complex forms. The component's customization options and event callbacks allow you to tailor its behavior to your specific needs.
