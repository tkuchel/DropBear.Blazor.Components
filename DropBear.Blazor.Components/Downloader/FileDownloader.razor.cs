#region

using System.Timers;
using Microsoft.AspNetCore.Components;
using Timer = System.Timers.Timer;

#endregion

namespace DropBear.Blazor.Components.Downloader;

public partial class FileDownloader : ComponentBase, IDisposable
{
    private bool _isCompleted;
    private bool _isDownloading;

    private int _progress;
    private Timer? _resetTimer;
    [Parameter] public string FileName { get; set; } = "example.pdf";
    [Parameter] public string FileType { get; set; } = "PDF";
    [Parameter] public string FileSize { get; set; } = "2.5 MB";
    [Parameter] public Func<IProgress<int>, Task<bool>>? OnDownload { get; set; }

    #region IDisposable Members

    public void Dispose()
    {
        _resetTimer?.Dispose();
    }

    #endregion

    protected override void OnInitialized()
    {
        _resetTimer = new Timer(2000);
        _resetTimer.Elapsed += ResetDownloadState!;
        _resetTimer.AutoReset = false;
    }

    private async Task StartDownload()
    {
        if (OnDownload is null)
        {
            throw new InvalidOperationException("OnDownload callback is not set");
        }

        _isDownloading = true;
        _progress = 0;
        _isCompleted = false;

        var progress = new Progress<int>(value =>
        {
            _progress = value;
            StateHasChanged();
        });

        var success = await OnDownload(progress);

        _isDownloading = false;
        _isCompleted = success;

        if (success)
        {
            _resetTimer?.Start();
        }

        StateHasChanged();
    }

    private void ResetDownloadState(object? sender, ElapsedEventArgs e)
    {
        _isCompleted = false;
        _progress = 0;
        _ = InvokeAsync(StateHasChanged);
    }
}
