![Nuget](https://img.shields.io/nuget/v/BlazorGenerator)

# BlazorEngine
You create the models and BlazorEngine will create the UI pages!
All the components are made from Microsoft Fluent UI. 


# What types of UI handle?
 1. List Pages
 2. Card Pages
 3. Worksheet Pages
 4. Two List (side-by-side) Pages
 5. Auto-add menu via Attribute
 6. Actions: functions decored with the "PageAction" attribute will be shown as buttons on the pages
 7. GridActions: functions decored with the "PageAction" attribute will be shown as buttons on datagrid rows
 8. Custom razor page: just use razor normally!

## Getting Started
Look at the Server/WebAssembly/Maui Project and see by yourself

### Note for NET MAUI
add in your index.html before the webassembly script
  `<script app-name="MauiApp1" src="./_content/Microsoft.FluentUI.AspNetCore.Components/js/initializersLoader.webview.js"></script>`

In the tag `<BlazorEngineApp>` add this parameter `BlazorEngineRenderMode="null"`

## Contributing
I'm a lonely programmer, feel free to open issues or submit Pull Requests!

