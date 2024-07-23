#region

using DropBear.Blazor.Components.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

#endregion

namespace DropBear.Blazor.Components.Components.Data;

public sealed partial class StandardDataGrid<TItem> : ComponentBase, IDisposable
    where TItem : class
{
    private DotNetObjectReference<StandardDataGrid<TItem>>? _dotNetHelper;

    private IJSObjectReference? _module;
    private object? _mouseMoveHandler;
    private object? _mouseUpHandler;
    private double _originalWidth;
    private double _resizeStartX;

    private DataGridColumn<TItem>? _resizingColumn;
    [Parameter] public RenderFragment? LoadingTemplate { get; set; }
    [Parameter] public RenderFragment? NoDataTemplate { get; set; }
    [Parameter] public RenderFragment<TItem>? ActionTemplate { get; set; }
    [Parameter] public ThemeType Theme { get; set; } = ThemeType.Light;
    [Parameter] public bool IsLoading { get; set; }
    [Parameter] public bool AllowSelection { get; set; } = true;
    [Parameter] public bool AllowColumnResize { get; set; } = true;
    [Parameter] public bool ShowPagination { get; set; } = true;
    [Parameter] public int PageSize { get; set; } = 10;
    [Parameter] public int CurrentPage { get; set; } = 1;
    [Parameter] public EventCallback<int> CurrentPageChanged { get; set; }
    [Parameter] public string SortColumn { get; set; } = "";
    [Parameter] public SortDirection SortDirection { get; set; } = SortDirection.Ascending;
    [Parameter] public EventCallback<(string, SortDirection)> SortChanged { get; set; }
    [Parameter] public Dictionary<string, string> Filters { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    [Parameter] public EventCallback<Dictionary<string, string>> FiltersChanged { get; set; }
    [Parameter] public EventCallback<HashSet<TItem>> SelectedItemsChanged { get; set; }
    [Parameter] public Func<TItem, string>? RowClass { get; set; }
    private HashSet<TItem> SelectedItems { get; set; } = [];
    private List<TItem> FilteredItems => ApplyFilters(Items);

    private List<TItem> PaginatedItems => FilteredItems
        .Skip((CurrentPage - 1) * PageSize)
        .Take(PageSize)
        .ToList();

    private int TotalPages => (int)Math.Ceiling(FilteredItems.Count / (double)PageSize);


    private bool AreAllSelected => Items is { Count: > 0 } && SelectedItems.Count == Items.Count;

    [Parameter] public EventCallback<TItem> RowClicked { get; set; }
    [Parameter] public bool AllowMultipleSelection { get; set; } = true;

    #region IDisposable

    /// <inheritdoc />
    public void Dispose()
    {
        _dotNetHelper?.Dispose();
        if (_module is IDisposable moduleDisposable)
        {
            moduleDisposable.Dispose();
        }
        else if (_module is not null)
        {
            _ = _module.DisposeAsync().AsTask();
        }
    }

    #endregion

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _module = await JsRuntime.InvokeAsync<IJSObjectReference>("eval", @"
                    const module = {
                        addGlobalEventListener: function (event, dotnetHelper, methodName) {
                            const handler = (e) => {
                                dotnetHelper.invokeMethodAsync(methodName, {
                                    clientX: e.clientX,
                                    clientY: e.clientY
                                });
                            };
                            window.addEventListener(event, handler);
                            return handler; // Return the handler for later removal
                        },
                        removeGlobalEventListener: function (event, handler) {
                            window.removeEventListener(event, handler);
                        }
                    };
                    module;
                ");
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_module is not null)
        {
            await _module.DisposeAsync();
        }
    }

    private bool IsSelected(TItem item)
    {
        return SelectedItems.Contains(item);
    }

    private async Task ToggleAllSelection()
    {
        if (AreAllSelected)
        {
            SelectedItems.Clear();
        }
        else
        {
            SelectedItems = [..Items];
        }

        await SelectedItemsChanged.InvokeAsync(SelectedItems);
    }

    private async Task ChangePage(int page)
    {
        if (page < 1 || page > TotalPages)
        {
            return;
        }

        CurrentPage = page;
        await CurrentPageChanged.InvokeAsync(CurrentPage);
        await InvokeAsync(StateHasChanged);
    }

    private async Task Sort(DataGridColumn<TItem> column)
    {
        if (!column.Sortable)
        {
            return;
        }

        if (SortColumn == column.PropertyName)
        {
            SortDirection = SortDirection is SortDirection.Ascending
                ? SortDirection.Descending
                : SortDirection.Ascending;
        }
        else
        {
            SortColumn = column.PropertyName;
            SortDirection = SortDirection.Ascending;
        }

        await SortChanged.InvokeAsync((SortColumn, SortDirection));
    }

    private string GetSortIconClass(DataGridColumn<TItem> column)
    {
        if (SortColumn != column.PropertyName)
        {
            return "";
        }

        return SortDirection is SortDirection.Ascending ? "fa-sort-up" : "fa-sort-down";
    }

    private async Task ApplyFilter(string columnName, string filterValue)
    {
        if (string.IsNullOrWhiteSpace(filterValue))
        {
            Filters.Remove(columnName);
        }
        else
        {
            Filters[columnName] = filterValue;
        }

        CurrentPage = 1; // Reset to first page when filter changes
        await FiltersChanged.InvokeAsync(Filters);
        await InvokeAsync(StateHasChanged);
    }

    private List<TItem> ApplyFilters(List<TItem> items)
    {
        if (Filters.Count is 0)
        {
            return items;
        }

        return items.Where(item =>
            Filters.All(filter =>
            {
                var column = Columns.Find(c => c.PropertyName == filter.Key);
                if (column is null)
                {
                    return true;
                }

                var value = column.PropertySelector?.Invoke(item).ToString();
                return string.IsNullOrEmpty(value) ||
                       value.Contains(filter.Value, StringComparison.OrdinalIgnoreCase);
            })
        ).ToList();
    }

    private async Task OnKeyDown(KeyboardEventArgs e)
    {
        if (Items.Count is 0)
        {
            return;
        }

        switch (e.Key)
        {
            case "ArrowDown":
                await MoveSelection(1);
                break;
            case "ArrowUp":
                await MoveSelection(-1);
                break;
            case "Home":
                await SelectFirstRow();
                break;
            case "End":
                await SelectLastRow();
                break;
            case " ": // Spacebar
                if (SelectedItems.Count == 1)
                {
                    await ToggleSelection(SelectedItems.First());
                }

                break;
        }
    }

    private async Task MoveSelection(int direction)
    {
        if (Items.Count == 0)
        {
            return;
        }

        var currentIndex = SelectedItems.Count == 1 ? Items.IndexOf(SelectedItems.First()) : -1;
        var newIndex = Math.Clamp(currentIndex + direction, 0, Items.Count - 1);

        await SelectSingleItem(Items[newIndex]);
    }

    private async Task SelectFirstRow()
    {
        if (Items.Count is not 0)
        {
            await SelectSingleItem(Items[0]);
        }
    }

    private async Task SelectLastRow()
    {
        if (Items.Count is not 0)
        {
            await SelectSingleItem(Items[^1]);
        }
    }

    private async Task SelectSingleItem(TItem item)
    {
        SelectedItems.Clear();
        SelectedItems.Add(item);
        await SelectedItemsChanged.InvokeAsync(SelectedItems);
        await InvokeAsync(StateHasChanged);
    }

    private async Task ToggleSelection(TItem item)
    {
        if (!SelectedItems.Remove(item))
        {
            SelectedItems.Add(item);
        }

        await SelectedItemsChanged.InvokeAsync(SelectedItems);
        await InvokeAsync(StateHasChanged);
    }

    private async Task OnRowClick(TItem item)
    {
        if (AllowSelection)
        {
            if (!AllowMultipleSelection)
            {
                await SelectSingleItem(item);
            }
            else
            {
                await ToggleSelection(item);
            }
        }

        await RowClicked.InvokeAsync(item);
    }

    private async Task StartColumnResize(MouseEventArgs e, DataGridColumn<TItem> column)
    {
        _resizingColumn = column;
        _resizeStartX = e.ClientX;
        _originalWidth = column.Width;

        _dotNetHelper = DotNetObjectReference.Create(this);

        // Add event listeners for mousemove and mouseup
        if (_module is not null)
        {
            _mouseMoveHandler =
                await _module.InvokeAsync<object>("addGlobalEventListener", "mousemove", _dotNetHelper,
                    "OnColumnResize");
            _mouseUpHandler =
                await _module.InvokeAsync<object>("addGlobalEventListener", "mouseup", _dotNetHelper,
                    "OnColumnResizeEnd");
        }
    }

    [JSInvokable]
    public async Task OnColumnResize(MouseEventArgs e)
    {
        if (_resizingColumn != null)
        {
            var diff = e.ClientX - _resizeStartX;
            _resizingColumn.Width = Math.Max(50, _originalWidth + diff); // Minimum width of 50px
            await InvokeAsync(StateHasChanged);
        }
    }

    [JSInvokable]
    public async Task OnColumnResizeEnd(MouseEventArgs e)
    {
        _resizingColumn = null;
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("removeGlobalEventListener", "mousemove", _mouseMoveHandler);
            await _module.InvokeVoidAsync("removeGlobalEventListener", "mouseup", _mouseUpHandler);
        }

        _dotNetHelper?.Dispose();
    }

    public Task ExportToCsv()
    {
        return Task.CompletedTask;
        // Implement CSV export
    }

    public Task ExportToExcel()
    {
        return Task.CompletedTask;
        // Implement Excel export
    }

#pragma warning disable CA1002
    [Parameter] public List<TItem> Items { get; set; } = [];
    [Parameter] public List<DataGridColumn<TItem>> Columns { get; set; } = [];
#pragma warning restore CA1002
}
