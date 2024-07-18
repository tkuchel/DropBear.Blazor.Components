# StandardDataGrid Component

The StandardDataGrid component in DropBear.Blazor.Components provides a flexible and customizable data grid for your
Blazor applications.

## Features

- Generic type support for binding to any data type
- Customizable columns with sorting
- Searching functionality
- Pagination
- Row selection with multi-select support
- Add, Edit, and Delete actions
- Light and dark mode
- Responsive design
- Accessibility support

## Installation

1. Install the NuGet package in your Blazor project:

```
dotnet add package DropBear.Blazor.Components
```

2. Add the following line to your `_Imports.razor` file:

```csharp
@using DropBear.Blazor.Components
```

## Usage

Here's a basic example of how to use the StandardDataGrid component:

```razor
<StandardDataGrid TItem="User"
                  Title="Users"
                  HeaderIcon="fas fa-users"
                  Items="@Users"
                  Columns="@Columns"
                  OnAddClick="AddUser"
                  OnEditClick="EditUser"
                  OnDeleteClick="DeleteUser"
                  OnSelectionChanged="HandleSelectionChanged"
                  IsLightMode="@IsLightMode" />

@code {
    private List<User> Users = new List<User>();
    private List<ColumnDefinition<User>> Columns = new List<ColumnDefinition<User>>
    {
        new ColumnDefinition<User> { Title = "First Name", Field = "FirstName", IconClass = "fas fa-user", ValueGetter = user => user.FirstName },
        new ColumnDefinition<User> { Title = "Last Name", Field = "LastName", IconClass = "fas fa-user", ValueGetter = user => user.LastName },
        new ColumnDefinition<User> { Title = "Email", Field = "Email", IconClass = "fas fa-envelope", ValueGetter = user => user.Email },
        // Add more columns as needed
    };

    private bool IsLightMode = false;

    protected override void OnInitialized()
    {
        Users = GetUsers(); // Implement this method to fetch your data
    }

    private void AddUser()
    {
        // Implement add logic
    }

    private void EditUser(User user)
    {
        // Implement edit logic
    }

    private void DeleteUser(User user)
    {
        // Implement delete logic
    }

    private void HandleSelectionChanged(List<User> selectedUsers)
    {
        // Handle selected users
    }
}
```

## Parameters

| Parameter          | Type                          | Default        | Description                                     |
|--------------------|-------------------------------|----------------|-------------------------------------------------|
| Title              | string                        | "Data Grid"    | The title displayed in the grid header          |
| HeaderIcon         | string                        | "fas fa-table" | The Font Awesome icon class for the header icon |
| Items              | IEnumerable<TItem>            | required       | The collection of items to display in the grid  |
| Columns            | List<ColumnDefinition<TItem>> | required       | The list of column definitions                  |
| AllowSearch        | bool                          | true           | Whether to show the search input                |
| AllowAdd           | bool                          | true           | Whether to show the add button                  |
| AllowEdit          | bool                          | true           | Whether to show the edit button for each row    |
| AllowDelete        | bool                          | true           | Whether to show the delete button for each row  |
| AllowSelect        | bool                          | true           | Whether to allow row selection                  |
| IsLightMode        | bool                          | false          | Whether to use light mode styling               |
| AddText            | string                        | "ADD ITEM"     | The text for the add button                     |
| OnAddClick         | EventCallback                 | -              | Event triggered when the add button is clicked  |
| OnEditClick        | EventCallback<TItem>          | -              | Event triggered when an edit button is clicked  |
| OnDeleteClick      | EventCallback<TItem>          | -              | Event triggered when a delete button is clicked |
| OnSelectionChanged | EventCallback<List<TItem>>    | -              | Event triggered when row selection changes      |

## ColumnDefinition<T> Properties

| Property    | Type            | Description                                        |
|-------------|-----------------|----------------------------------------------------|
| Title       | string          | The column header text                             |
| Field       | string          | The field name for sorting                         |
| IconClass   | string          | The Font Awesome icon class for the column header  |
| ValueGetter | Func<T, object> | A function to get the display value for the column |

## Styling

The StandardDataGrid component uses CSS variables for easy customization. You can override these variables in your own
CSS to change the appearance of the grid:

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
}
```

## Customization Examples

### Custom Column Rendering

You can customize how a column is rendered by providing a custom `ValueGetter`:

```csharp
new ColumnDefinition<User> 
{ 
    Title = "Full Name", 
    Field = "FullName", 
    IconClass = "fas fa-user", 
    ValueGetter = user => $"{user.FirstName} {user.LastName}" 
}
```

### Conditional Styling

You can apply conditional styling to rows by adding a class based on a condition:

```razor
<div class="datagrid-row @(item.IsActive ? "active-row" : "inactive-row")" role="row">
    <!-- Row content -->
</div>
```

Then add the corresponding styles in your CSS:

```css
.active-row .datagrid-cell {
    font-weight: bold;
}

.inactive-row .datagrid-cell {
    opacity: 0.7;
}
```

## Accessibility

The StandardDataGrid component is designed with accessibility in mind:

- It uses semantic HTML elements and ARIA attributes
- It supports keyboard navigation
- It provides clear labeling for interactive elements

## Browser Support

This component is compatible with all modern browsers, including the latest versions of Chrome, Firefox, Safari, and
Edge.

## Conclusion

The StandardDataGrid component provides a powerful and flexible way to display and interact with tabular data in your
Blazor applications. By leveraging its customization options and event callbacks, you can create tailored data grid
experiences that fit your specific needs.
