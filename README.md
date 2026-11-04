# BlazorEngine

BlazorEngine is a library that lets you generate full-featured Blazor UI pages from your C# models and explicit configuration. It is
designed for rapid development of CRUD-style apps, admin panels, and internal tools, with minimal boilerplate.

## Features

- **Automatic UI generation**: List, Card, Worksheet, and Two-List (side-by-side) pages are generated from your models.
- **Typed menu registration**: Register pages with `options.AddMenuItem<TPage>(...)` in the host. No assembly scans,
  metadata files, generated initializers, or custom MSBuild tasks are required.
- **Action buttons**: Override `CreatePageActions()` and `CreateGridActions()` with typed delegates. Action errors
  propagate to the existing error boundary.
- **Expression-based fields**: Declare fields using `fields.Add(x => x.Name)` and fluent customization. Existing
  model display captions remain supported.
- **Icon instances**: Pass icons directly, such as `new Size20.Run()`, without resolving icon types from metadata.
- **Existing UI behavior**: Selection, button visibility, dialogs, Excel export, keyboard refresh, background tasks,
  themes, and session-aware `ISecurity` checks remain supported.
- **Custom Razor pages**: You can still write custom Razor components/pages as needed.

## How to Use

1. **Reference BlazorEngine in your Blazor project** (WebAssembly, Server, or MAUI Blazor).
2. **Configure a page explicitly**. Existing layout namespaces and lifecycle hooks are retained:

```csharp
using BlazorEngine.Actions;
using BlazorEngine.Fields;
using BlazorEngine.Layouts;
using Microsoft.AspNetCore.Components;

public class Product
{
  public string Name { get; set; } = string.Empty;
}

[Route(Route)]
public class Products : ListPage<Product>
{
  public const string Route = "/products";
  public override string Title => "Products";

  protected override Task ConfigureFields(FieldListBuilder<Product> fields)
  {
    fields.Add(x => x.Name).Caption("Product name").Required();
    return Task.CompletedTask;
  }

  protected override Task LoadData()
  {
    Content = new[] { new Product { Name = "Example" } };
    return Task.CompletedTask;
  }

  protected override IReadOnlyList<PageActionDefinition> CreatePageActions() =>
    [new("Refresh", RefreshAsync)];

  private async Task RefreshAsync()
  {
    await LoadData();
    StateHasChanged();
  }
}
```

3. **Register menu entries in your host**. Blazor's normal Router still handles page routes; a page does not have
   to be in the menu to be routable or used as a modal/edit form.

```csharp
builder.Services.AddBlazorEngine(options =>
{
  options.AddMenuItem<Products>("Products", Products.Route);
  options.AddFooterItem("Issue?", "https://github.com/f4n0/BlazorGenerator", openInNewTab: true);
});
```

Registrations default to full permissions and can restrict them with `PermissionFlags` from `BlazorEngine.Security`.
These flags are intersected with the existing `ISecurity` provider's permissions; they never override a provider denial.
The legacy permission model has no separate read gate, so `Read` is reserved rather than adding new access behavior.
Menu order defaults to 999; equally ordered items retain registration order. Action icons keep the existing Filled
Run default, and menu icons keep the Filled Balloon default.

4. **See the Demo**: `Demo/TestShared/DemoRegistration.cs` is the single registration block shared by all four demo
   hosts. The `Demo/TestShared` project contains sample models and demonstrates all features. Explore it for
   real-world usage.

## Getting Started

- Clone this repository.
- Open the solution in Visual Studio.
- Set one of the demo projects (WebAssembly, Server, or MAUI) as the startup project.
- Run (F5) and explore the generated UI.

### Special Note for .NET MAUI

Add this script to your `index.html` before the WebAssembly script:

```html

<script app-name="MauiApp1"
        src="./_content/Microsoft.FluentUI.AspNetCore.Components/js/initializersLoader.webview.js"></script>
```

In the `<BlazorEngineApp>` tag, add the parameter: `BlazorGenRenderMode="null"`

## Contributing

Contributions are welcome! Open issues or submit pull requests if you have ideas or improvements.

