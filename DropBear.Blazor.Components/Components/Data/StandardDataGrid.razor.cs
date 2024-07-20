#region

using DropBear.Blazor.Components.Components.Menus;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

#endregion

namespace DropBear.Blazor.Components.Components.Data;

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
    [Parameter] public RenderFragment? EmptyTemplate { get; set; }
    [Parameter] public RenderFragment? LoadingTemplate { get; set; }
    [Parameter] public bool IsLoading { get; set; }

    private string SearchTerm { get; set; } = "";
    private int CurrentPage { get; set; } = 1;
    private int ItemsPerPage { get; set; } = 10;
    private string SortColumn { get; set; } = string.Empty;
    private bool IsSortAscending { get; set; } = true;
    private HashSet<TItem> SelectedItems { get; } = new();
#pragma warning disable CA1002
    [Parameter] public List<ContextMenuItem> ContextMenuItems { get; set; } = new();
#pragma warning restore CA1002
    [Parameter] public EventCallback<(ContextMenuItem, TItem)> OnContextMenuItemClick { get; set; }
    [Parameter] public string ContextMenuBackgroundColor { get; set; } = "#2b2d31";
    [Parameter] public string ContextMenuTextColor { get; set; } = "#a4b1cd";
    [Parameter] public string ContextMenuHighlightColor { get; set; } = "#4ebafd";

    private StandardContextMenu ContextMenu { get; set; } = new();
    private TItem CurrentContextMenuItem { get; set; } = default!;

    private IEnumerable<TItem> FilteredAndSortedItems
    {
        get
        {
            if (Items is null || !Items.Any())
            {
                return Array.Empty<TItem>();
            }

            var query = Items.AsEnumerable();

            // Apply search filter
            if (!string.IsNullOrEmpty(SearchTerm) && Columns is not null && Columns.Count is not 0)
            {
                query = query.Where(item => Columns.Exists(c =>
                {
                    var value = c.ValueGetter(item);
                    return value is not null &&
                           value.ToString()!.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase);
                }));
            }

            // Apply sorting
            if (!string.IsNullOrEmpty(SortColumn) && Columns is not null)
            {
                var sortColumn = Columns.Find(c => c.Field == SortColumn);
                if (sortColumn is not null)
                {
                    query = IsSortAscending
                        ? query.OrderBy(item => sortColumn.ValueGetter(item) ?? string.Empty)
                        : query.OrderByDescending(item => sortColumn.ValueGetter(item) ?? string.Empty);
                }
            }

            // Apply pagination
            return query
                .Skip((CurrentPage - 1) * ItemsPerPage)
                .Take(ItemsPerPage)
                .ToList();
        }
    }

    private int TotalPages => Items is null ? 0 : (int)Math.Ceiling(Items.Count() / (double)ItemsPerPage);
    private bool CanGoToPreviousPage => CurrentPage > 1;
    private bool CanGoToNextPage => CurrentPage < TotalPages;

    private bool AreAllSelected
    {
        get => Items is not null && SelectedItems.Count == Items.Take(SelectedItems.Count + 1).Count();
        set
        {
            SelectedItems.Clear();
            if (value && Items is not null)
            {
                foreach (var item in Items)
                {
                    SelectedItems.Add(item);
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

    protected override void OnParametersSet()
    {
        if (Items is not null && !Items.Any())
        {
            CurrentPage = 1;
        }
    }

    private async Task ShowContextMenu(MouseEventArgs e, TItem item)
    {
        CurrentContextMenuItem = item;
        await ContextMenu.Show(e.ClientX, e.ClientY);
    }

    private async Task HandleContextMenuItemClick(ContextMenuItem menuItem)
    {
        await OnContextMenuItemClick.InvokeAsync((menuItem, CurrentContextMenuItem));
    }
}
