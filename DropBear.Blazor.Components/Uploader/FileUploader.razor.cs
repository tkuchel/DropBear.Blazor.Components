#region

using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;

#endregion

namespace DropBear.Blazor.Components.Uploader;

public partial class FileUploader : ComponentBase
{
    private readonly List<FileDetails> _files = [];
    private bool _dragOver;
    private int _overallProgress;
    private bool _uploading;

    [Parameter] public Func<IBrowserFile, IProgress<int>, Task<bool>>? OnFileUpload { get; set; }

    private async Task HandleDrop(DragEventArgs e)
    {
        _dragOver = false;
        await HandleDroppedFiles(e);
    }

    private async Task HandleDroppedFiles(DragEventArgs e)
    {
        var files = await JSRuntime.InvokeAsync<string[]>("getFilesFromDataTransfer", null);
        foreach (var file in files)
        {
            var fileInfo = JsonSerializer.Deserialize<FileInfo>(file);
            if (fileInfo is not null)
            {
                _files.Add(new FileDetails
                {
                    Name = fileInfo.Name,
                    Size = fileInfo.Size,
                    ContentType = fileInfo.Type,
                    Status = "Ready to upload"
                });
            }
        }

        StateHasChanged();
    }

    private Task HandleFileSelection(InputFileChangeEventArgs e)
    {
        foreach (var file in e.GetMultipleFiles())
        {
            _files.Add(new FileDetails
            {
                Name = file.Name,
                Size = file.Size,
                ContentType = file.ContentType,
                File = file,
                Status = "Ready to upload"
            });
        }

        StateHasChanged();
        return Task.CompletedTask;
    }

    private void RemoveFile(FileDetails file)
    {
        _files.Remove(file);
        StateHasChanged();
    }

    private static string FormatFileSize(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
        var order = 0;
        while (bytes >= 1024 && order < sizes.Length - 1)
        {
            order++;
            bytes = bytes / 1024;
        }

        return $"{bytes:0.##} {sizes[order]}";
    }

    private async Task UploadFiles()
    {
        if (OnFileUpload == null)
        {
            throw new InvalidOperationException("OnFileUpload callback is not set");
        }

        _uploading = true;
        _overallProgress = 0;
        var totalFiles = _files.Count;
        var uploadedFiles = 0;

        foreach (var file in _files)
        {
            if (file.File == null)
            {
                continue;
            }

            file.Status = "Uploading...";
            StateHasChanged();

            var progress = new Progress<int>(percentComplete =>
            {
                file.Progress = percentComplete;
                UpdateOverallProgress();
            });

            try
            {
                var success = await OnFileUpload(file.File, progress);
                file.Status = success ? "Uploaded" : "Failed";
            }
            catch (Exception)
            {
                file.Status = "Failed";
            }

            uploadedFiles++;
            UpdateOverallProgress();
        }

        _uploading = false;
        await Task.Delay(2000); // Give user time to see the final status
        _files.Clear();
        _overallProgress = 0;
        StateHasChanged();
    }

    private void UpdateOverallProgress()
    {
        var totalProgress = _files.Sum(f => f.Progress);
        _overallProgress = totalProgress / _files.Count;
        StateHasChanged();
    }

    #region Nested type: FileDetails

    private sealed class FileDetails
    {
        public string Name { get; set; } = string.Empty;
        public long Size { get; set; }
        public string ContentType { get; set; } = string.Empty;
        public string Status { get; set; } = "Ready to upload";
        public IBrowserFile? File { get; set; }
        public int Progress { get; set; }
    }

    #endregion

    #region Nested type: FileInfo

    private sealed class FileInfo
    {
        public string Name { get; set; } = string.Empty;
        public long Size { get; set; }
        public string Type { get; set; } = string.Empty;
    }

    #endregion
}
