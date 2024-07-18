# FileDownloader Component

The FileDownloader component in DropBear.Blazor.Components provides a flexible and customizable file download interface
for your Blazor applications.

## Features

- Clean, user-friendly interface for file downloads
- Progress bar to show download status
- Customizable download process via callback
- File information display (name, type, size)
- Visual feedback for download completion
- Automatic UI reset after successful download
- Smooth animations and transitions

## Installation

1. Add the FileDownloader.razor and FileDownloader.razor.css files to your Blazor project's Components folder.
2. Ensure your _Imports.razor file includes the following line:
   ```csharp
   @using DropBear.Blazor.Components
   ```

## Usage

```html
@page "/"
@inject HttpClient HttpClient

<FileDownloader 
    FileName="example.pdf"
    FileType="PDF"
    FileSize="2.5 MB"
    OnDownload="DownloadFile" />

@code {
    private async Task<bool> DownloadFile(IProgress<int> progress)
    {
        try
        {
            // Simulating file download with progress
            for (int i = 0; i <= 100; i += 10)
            {
                await Task.Delay(500); // Simulate network delay
                progress.Report(i);
            }

            // Actual file download logic here
            // Example: await JSRuntime.InvokeVoidAsync("saveAsFile", "example.pdf", fileContent);

            return true;
        }
        catch
        {
            return false;
        }
    }
}
```

## Parameters

| Parameter  | Type                             | Default       | Description                                      |
|------------|----------------------------------|---------------|--------------------------------------------------|
| FileName   | string                           | "example.pdf" | The name of the file to be downloaded            |
| FileType   | string                           | "PDF"         | The type of the file (e.g., PDF, DOCX)           |
| FileSize   | string                           | "2.5 MB"      | The size of the file                             |
| OnDownload | Func<IProgress<int>, Task<bool>> | null          | Callback function for handling the file download |

## Events

| Event      | Description                                   |
|------------|-----------------------------------------------|
| OnDownload | Triggered when the download button is clicked |

## Methods

The FileDownloader component doesn't expose any public methods. The download process is initiated by user interaction
and handled through the OnDownload callback.

## Styling

The FileDownloader component uses CSS variables for easy customization. You can override these variables in your own CSS
to change the appearance of the downloader:

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

### Custom Download Handler with Error Handling

```html
<FileDownloader 
    FileName="large-file.zip"
    FileType="ZIP"
    FileSize="1.2 GB"
    OnDownload="CustomDownloadHandler" />

@code {
    private async Task<bool> CustomDownloadHandler(IProgress<int> progress)
    {
        try
        {
            // Implement your custom download logic here
            // Use the progress parameter to report download progress
            for (int i = 0; i <= 100; i += 5)
            {
                await Task.Delay(200);
                progress.Report(i);
                if (i == 50) throw new Exception("Simulated download error");
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Download failed: {ex.Message}");
            return false;
        }
    }
}
```

### Integration with File List

```html
@foreach (var file in fileList)
{
    <FileDownloader 
        FileName="@file.Name"
        FileType="@file.Type"
        FileSize="@file.Size"
        OnDownload="@(progress => DownloadFile(file, progress))" />
}

@code {
    private List<FileInfo> fileList = new List<FileInfo>
    {
        new FileInfo { Name = "document.pdf", Type = "PDF", Size = "1.5 MB" },
        new FileInfo { Name = "image.jpg", Type = "JPG", Size = "2.3 MB" },
        // Add more files as needed
    };

    private async Task<bool> DownloadFile(FileInfo file, IProgress<int> progress)
    {
        // Implement download logic for each file
        // Use the file information and report progress
        // Return true if download is successful, false otherwise
    }
}
```

## Accessibility

The FileDownloader component is designed with accessibility in mind:

- It uses semantic HTML elements for better screen reader support
- The download button is keyboard accessible
- ARIA attributes are used to provide context for assistive technologies

## Browser Support

This component is compatible with all modern browsers, including the latest versions of Chrome, Firefox, Safari, and
Edge.

## Conclusion

The FileDownloader component provides a user-friendly and customizable way to handle file downloads in your Blazor
application. By leveraging the `OnDownload` callback, you can easily integrate it with your existing backend services or
custom download logic. The component's progress reporting, visual feedback, and customizable styling make it a versatile
choice for various file download scenarios.
