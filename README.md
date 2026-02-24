
# BlazorEngine

BlazorEngine is a library that lets you generate full-featured Blazor UI pages from your C# models and attributes. It is designed for rapid development of CRUD-style apps, admin panels, and internal tools, with minimal boilerplate.

## Features

- **Automatic UI generation**: List, Card, Worksheet, and Two-List (side-by-side) pages are generated from your models.
- **Menu auto-discovery**: Add a simple attribute to your model and it appears in the navigation menu—no manual menu editing required.
- **Action buttons**: Decorate methods with `[PageAction]` or `[GridAction]` to expose them as buttons on pages or in data grids.
- **Attribute-driven customization**: Control visibility, grouping, and more with attributes on your models and properties.
- **No icons required**: Menu entries are plain text by default for simplicity.
- **Custom Razor pages**: You can still write custom Razor components/pages as needed.

## How to Use

1. **Reference BlazorEngine in your Blazor project** (WebAssembly, Server, or MAUI Blazor).
2. **Add attributes to your models** to control menu entries and actions. Example:

```csharp
[AddToMenu(Title = "Products", Route = "/products")]
public class Product { /* ... */ }

[PageAction(Caption = "Refresh")]
public void Refresh() { /* ... */ }
```

3. **Run your app**. The UI and menu are generated automatically.

4. **See the Demo**: The `Demo/TestShared` project contains sample models and demonstrates all features. Explore it for real-world usage.

## Getting Started

- Clone this repository.
- Open the solution in Visual Studio.
- Set one of the demo projects (WebAssembly, Server, or MAUI) as the startup project.
- Run (F5) and explore the generated UI.

### Special Note for .NET MAUI
Add this script to your `index.html` before the WebAssembly script:

```html
<script app-name="MauiApp1" src="./_content/Microsoft.FluentUI.AspNetCore.Components/js/initializersLoader.webview.js"></script>
```

In the `<BlazorEngineApp>` tag, add the parameter: `BlazorEngineRenderMode="null"`

## Contributing

Contributions are welcome! Open issues or submit pull requests if you have ideas or improvements.

