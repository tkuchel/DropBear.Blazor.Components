# FileUploader Component

The FileUploader component in DropBear.Blazor.Components provides a flexible and customizable file upload interface for
your Blazor applications.

## Features

- Drag and drop file upload support
- Multiple file selection
- Customizable upload process via callback
- Progress reporting for individual files and overall upload
- File size and type information display
- Ability to remove selected files before upload
- Smooth animations and user-friendly interface
- Event callbacks for file selection and upload completion

## Installation

1. Add the FileUploader.razor and FileUploader.razor.css files to your Blazor project's Components folder.
2. Ensure your _Imports.razor file includes the following line:
   ```csharp
   @using DropBear.Blazor.Components.Uploader
   ```

## Usage

```html
@using DropBear.Blazor.Components.Uploader
@inject HttpClient HttpClient

<FileUploader OnFileUpload="UploadFile" />

@code {
    private async Task<bool> UploadFile(IBrowserFile file, IProgress<int> progress)
    {
        try
        {
            var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(file.OpenReadStream(MaxAllowedSize));
            content.Add(fileContent, "file", file.Name);

            var response = await HttpClient.PostAsync("api/upload", content);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    private const long MaxAllowedSize = 1024 * 1024 * 10; // 10 MB
}
```

## Parameters

| Parameter    | Type                                           | Default | Description                                 |
|--------------|------------------------------------------------|---------|---------------------------------------------|
| OnFileUpload | Func<IBrowserFile, IProgress<int>, Task<bool>> | null    | Callback function for handling file uploads |

## Events

| Event        | Description                                            |
|--------------|--------------------------------------------------------|
| OnFileUpload | Triggered for each file when the upload process starts |

## Methods

The FileUploader component doesn't expose any public methods. File selection and upload processes are handled
internally.

## Styling

The FileUploader component uses CSS variables for easy customization. You can override these variables in your own CSS
to change the appearance of the uploader:

```css
:root {
  --clr-background: #1e1f22;
  --clr-surface: #2b2d31;
  --clr-surface-light: #3a3d44;
  --clr-surface-lighter: #454950;
  --clr-primary: #4ebafd;
  --clr-secondary: #8552f8;
  --clr-success: #7cd651;
  --clr-warning: #ffd14d;
  --clr-error: #ff5757;
  --clr-text: #ffffff;
  --clr-text-muted: #a4b1cd;
  --clr-overlay: rgba(0, 0, 0, 0.7);
  --border-radius: 8px;
  --transition: all 0.3s ease;
}
```

## Customization Examples

### Custom Upload Handler

```html
<FileUploader OnFileUpload="CustomUploadHandler" />

@code {
    private async Task<bool> CustomUploadHandler(IBrowserFile file, IProgress<int> progress)
    {
        // Implement your custom upload logic here
        // Use the progress parameter to report upload progress
        for (int i = 0; i <= 100; i += 10)
        {
            await Task.Delay(100);
            progress.Report(i);
        }
        return true;
    }
}
```

### Integration with Form Submission

```html
<EditForm Model="@formModel" OnValidSubmit="HandleValidSubmit">
    <DataAnnotationsValidator />
    <ValidationSummary />

    <InputText @bind-Value="formModel.Name" />
    
    <FileUploader OnFileUpload="UploadFile" />

    <button type="submit">Submit</button>
</EditForm>

@code {
    private FormModel formModel = new();
    private List<string> uploadedFiles = new();

    private async Task<bool> UploadFile(IBrowserFile file, IProgress<int> progress)
    {
        // Implement file upload logic
        // If successful, add the file to the list of uploaded files
        uploadedFiles.Add(file.Name);
        return true;
    }

    private async Task HandleValidSubmit()
    {
        // Process form submission along with uploaded files
        await SubmitFormWithFiles(formModel, uploadedFiles);
    }
}
```

## Accessibility

The FileUploader component is designed with accessibility in mind:

- It uses semantic HTML elements for better screen reader support
- Keyboard navigation is supported for file selection and removal
- ARIA attributes are used to provide context for assistive technologies

## Browser Support

This component is compatible with all modern browsers, including the latest versions of Chrome, Firefox, Safari, and
Edge.

## Conclusion

The FileUploader component provides a flexible and user-friendly way to handle file uploads in your Blazor application.
By leveraging the `OnFileUpload` callback, you can easily integrate it with your existing backend services or custom
upload logic. The component's drag-and-drop support, progress reporting, and customizable styling make it a versatile
choice for various file upload scenarios.
