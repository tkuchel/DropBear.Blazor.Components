#region

using DropBear.Blazor.Components.Helpers;
using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Components.Data;

public partial class DataGrid<TItem> : ComponentBase where TItem : notnull
{
    [Parameter] public string Title { get; set; } = "Data Grid";
    [Parameter] public string ItemName { get; set; } = "Item";
    [Parameter] public bool AllowAdd { get; set; } = true;
    [Parameter] public bool AllowEdit { get; set; } = true;
    [Parameter] public bool AllowDelete { get; set; } = true;
    [Parameter] public bool AllowSelect { get; set; } = true;
    [Parameter] public EventCallback<TItem> OnAddItem { get; set; }
    [Parameter] public EventCallback<TItem> OnEditItem { get; set; }
    [Parameter] public EventCallback<TItem> OnDeleteItem { get; set; }

    private string SearchTerm { get; set; } = "";
    private bool IsLoading { get; }
    private int CurrentPage { get; set; } = 1;
    private int ItemsPerPage { get; set; } = 10;
    private string SortColumn { get; set; } = "";
    private bool IsSortAscending { get; set; } = true;
    private Dictionary<TItem, bool> SelectedItems { get; } = new();

    private bool AreAllSelected => Items.TrueForAll(IsItemSelected);

    private IEnumerable<TItem> DisplayedItems => Items
        .Where(FilterItem)
        .OrderByDynamic(SortItems)
        .Skip((CurrentPage - 1) * ItemsPerPage)
        .Take(ItemsPerPage);

    private int TotalPages => (int)Math.Ceiling(Items.Count(FilterItem) / (double)ItemsPerPage);
    private bool CanGoToPreviousPage => CurrentPage > 1;
    private bool CanGoToNextPage => CurrentPage < TotalPages;

    private bool FilterItem(TItem item)
    {
        if (string.IsNullOrWhiteSpace(SearchTerm))
        {
            return true;
        }

        return Columns.Exists(column =>
        {
            var value = column.ValueGetter(item);
            return value?.ToString()?.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) is true;
        });
    }

    private Func<TItem, object> SortItems()
    {
        if (string.IsNullOrEmpty(SortColumn))
        {
            return item => item; // Default sort, you might want to adjust this based on your needs
        }

        var column = Columns.First(c => c.Field == SortColumn);
        return column.ValueGetter!;
    }

    private void SortBy(ColumnDefinition<TItem> column)
    {
        if (!column.Sortable)
        {
            return;
        }

        if (SortColumn == column.Field)
        {
            IsSortAscending = !IsSortAscending;
        }
        else
        {
            SortColumn = column.Field;
            IsSortAscending = true;
        }
    }

    private void PreviousPage()
    {
        if (CanGoToPreviousPage)
        {
            CurrentPage--;
        }
    }

    private void NextPage()
    {
        if (CanGoToNextPage)
        {
            CurrentPage++;
        }
    }

    private async Task OnAddClick()
    {
        await OnAddItem.InvokeAsync(default);
    }

    private async Task OnEditClick(TItem item)
    {
        await OnEditItem.InvokeAsync(item);
    }

    private async Task OnDeleteClick(TItem item)
    {
        await OnDeleteItem.InvokeAsync(item);
    }

    private bool IsItemSelected(TItem item)
    {
        return SelectedItems.ContainsKey(item) && SelectedItems[item];
    }

    private void ToggleSelectAll(ChangeEventArgs e)
    {
        var isSelected = (bool)(e.Value ?? false);
        foreach (var item in Items)
        {
            SelectedItems[item] = isSelected;
        }
    }

    private void ToggleItemSelection(TItem item)
    {
        if (SelectedItems.TryGetValue(item, out var value))
        {
            SelectedItems[item] = SelectedItems[item] = !value;
        }
        else
        {
            SelectedItems[item] = true;
        }
    }

    public IEnumerable<TItem> GetSelectedItems()
    {
        return SelectedItems.Where(kvp => kvp.Value).Select(kvp => kvp.Key);
    }
#pragma warning disable CA1002
    [Parameter] public List<TItem> Items { get; set; } = [];

    [Parameter] public List<ColumnDefinition<TItem>> Columns { get; set; } = [];
#pragma warning restore CA1002
}
