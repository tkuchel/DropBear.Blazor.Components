#region

using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

#endregion

namespace DropBear.Blazor.Components.Components.Uploader;

public partial class FileUploader : ComponentBase, IAsyncDisposable
{
    private readonly List<FileDetails> _files = [];
    private DotNetObjectReference<FileUploader>? _dotNetReference;
    private bool _dragOver;
    private int _overallProgress;
    private bool _uploading;

    [Parameter] public Func<IBrowserFile, IProgress<int>, Task<bool>>? OnFileUpload { get; set; }
    [Parameter] public bool IsLightMode { get; set; }

    #region IAsyncDisposable Members

    public async ValueTask DisposeAsync()
    {
        _dotNetReference?.Dispose();
        await JsRuntime.InvokeVoidAsync("eval",
            "document.removeEventListener('drop', window.fileUploaderInterop.handleDrop);");
    }

    #endregion

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _dotNetReference = DotNetObjectReference.Create(this);
            await InitializeJavaScript();
        }
    }

    private async Task InitializeJavaScript()
    {
        await JsRuntime.InvokeVoidAsync("eval", @"
                    window.fileUploaderInterop = {
                        getFilesFromDataTransfer: function(dataTransfer) {
                            return new Promise(resolve => {
                                const files = [];
                                if (dataTransfer.items) {
                                    for (let i = 0; i < dataTransfer.items.length; i++) {
                                        if (dataTransfer.items[i].kind === 'file') {
                                            const file = dataTransfer.items[i].getAsFile();
                                            files.push(JSON.stringify({
                                                name: file.name,
                                                size: file.size,
                                                type: file.type
                                            }));
                                        }
                                    }
                                } else {
                                    for (let i = 0; i < dataTransfer.files.length; i++) {
                                        const file = dataTransfer.files[i];
                                        files.push(JSON.stringify({
                                            name: file.name,
                                            size: file.size,
                                            type: file.type
                                        }));
                                    }
                                }
                                resolve(files);
                            });
                        },
                        addDropListener: function(dotNetReference) {
                            document.addEventListener('drop', async function(e) {
                                e.preventDefault();
                                const files = await window.fileUploaderInterop.getFilesFromDataTransfer(e.dataTransfer);
                                await dotNetReference.invokeMethodAsync('HandleDroppedFilesFromJS', files);
                            });
                        }
                    };
                ");

        await JsRuntime.InvokeVoidAsync("fileUploaderInterop.addDropListener", _dotNetReference);
    }

    private void HandleDragEnter()
    {
        _dragOver = true;
    }

    private void HandleDragLeave()
    {
        _dragOver = false;
    }

    [JSInvokable]
    public Task HandleDroppedFilesFromJs(string[] files)
    {
        _dragOver = false;
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
        return Task.CompletedTask;
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
