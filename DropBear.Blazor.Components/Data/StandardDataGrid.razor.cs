#region

using Microsoft.AspNetCore.Components;

#endregion

namespace DropBear.Blazor.Components.Data;

public partial class StandardDataGrid<TItem>
{
    [Parameter] public string Title { get; set; } = "Data Grid";
    [Parameter] public string HeaderIcon { get; set; } = "fas fa-table";
    [Parameter] public IEnumerable<TItem>? Items { get; set; }
#pragma warning disable CA1002
    [Parameter] public List<ColumnDefinition<TItem>>? Columns { get; set; }
#pragma warning restore CA1002
    [Parameter] public bool AllowSearch { get; set; } = true;
    [Parameter] public bool AllowAdd { get; set; } = true;
    [Parameter] public bool AllowEdit { get; set; } = true;
    [Parameter] public bool AllowDelete { get; set; } = true;
    [Parameter] public bool AllowSelect { get; set; } = true;
    [Parameter] public bool IsLightMode { get; set; }
    [Parameter] public string AddText { get; set; } = "ADD ITEM";
    [Parameter] public EventCallback OnAddClick { get; set; }
    [Parameter] public EventCallback<TItem> OnEditClick { get; set; }
    [Parameter] public EventCallback<TItem> OnDeleteClick { get; set; }
    [Parameter] public EventCallback<List<TItem>> OnSelectionChanged { get; set; }

    private string SearchTerm { get; set; } = "";
    private int CurrentPage { get; set; } = 1;
    private int ItemsPerPage { get; set; } = 10;
    private string SortColumn { get; set; } = string.Empty;
    private bool IsSortAscending { get; set; } = true;
    private HashSet<TItem> SelectedItems { get; } = [];

    private IEnumerable<TItem> FilteredAndSortedItems
    {
        get
        {
            if (Items is not null)
            {
                return Items
                    .Where(item => Columns is not null && (string.IsNullOrEmpty(SearchTerm) || Columns.Exists(c =>
                        c.ValueGetter(item)!.ToString()!.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))))
                    .OrderBy(
                        item =>
                        {
                            return Columns?.First(c => c.Field == SortColumn).ValueGetter(item);
                        })
                    .Skip((CurrentPage - 1) * ItemsPerPage)
                    .Take(ItemsPerPage);
            }

            return Array.Empty<TItem>();
        }
    }

    private int TotalPages
    {
        get
        {
            if (Items is not null)
            {
                return (int)Math.Ceiling(Items.Count() / (double)ItemsPerPage);
            }

            return 0;
        }
    }

    private bool CanGoToPreviousPage => CurrentPage > 1;
    private bool CanGoToNextPage => CurrentPage < TotalPages;

    private bool AreAllSelected
    {
        get => Items is not null && SelectedItems.Count == Items.Take(SelectedItems.Count + 1).Count();
        set
        {
            SelectedItems.Clear();
            if (value)
            {
                if (Items is not null)
                {
                    foreach (var item in Items)
                    {
                        SelectedItems.Add(item);
                    }
                }
            }

            _ = OnSelectionChanged.InvokeAsync(SelectedItems.ToList());
        }
    }

    private bool IsItemSelected(TItem item)
    {
        return SelectedItems.Contains(item);
    }

    private void ToggleItemSelection(TItem item)
    {
        if (!SelectedItems.Add(item))
        {
            SelectedItems.Remove(item);
        }

        _ = OnSelectionChanged.InvokeAsync(SelectedItems.ToList());
    }

    private void SortByColumn(ColumnDefinition<TItem> column)
    {
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

    private string GetSortOrder(ColumnDefinition<TItem> column)
    {
        if (SortColumn != column.Field)
        {
            return "none";
        }

        return IsSortAscending ? "ascending" : "descending";
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
}
