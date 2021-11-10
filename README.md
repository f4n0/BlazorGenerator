# Eos.BlazorGenerator
It will autogenerate at runtime blazor pages according to attributes and properties

## Getting Started
Edit `_Imports.razor` and add: `@using Eos.Blazor.Generator.Components`

Edit `App.razor`:
```
<Router AppAssembly="@typeof(Program).Assembly" PreferExactMatches="@true">
        <Found Context="routeData">
            <DynamicRoute RouteData="@routeData" DefaultLayout="@typeof(DynamicMainLayout)" />
        </Found>
        <NotFound>
            <LayoutView Layout="@typeof(DynamicMainLayout)">
                <p>Sorry, there's nothing at this address.</p>
            </LayoutView>
        </NotFound>
    </Router>
```
    
In `Startup.cs` add BlazorGen Services:`services.AddEosBlazorGen();`

BlazorGen use Blazorise for its components, so edit the `_Host.cshtml` adding:
#### In Head tag
```<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.0/dist/css/bootstrap.min.css" integrity="sha384-B0vP5xmATw1+K9KRQjQERJvTumQW0nPEzvF6L/Z6nronJ3oUOFUFpCjEUQouq2+l" crossorigin="anonymous">
<link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.12.0/css/all.css">
<link href="_content/Blazorise/blazorise.css" rel="stylesheet" />
<link href="_content/Blazorise.Bootstrap/blazorise.bootstrap.css" rel="stylesheet" />
```
#### Before body closing:    
```<script src="https://code.jquery.com/jquery-3.5.1.slim.min.js" integrity="sha384-DfXdz2htPH0lsSSs5nCTpuj/zy4C+OGpamoFVy38MVBnE+IbbVYUew+OrCXaRkfj" crossorigin="anonymous"></script>
<script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.min.js" integrity="sha384-9/reFTGAW83EW2RDu2S0VKaIzap3H66lZH81PoYlFhbGU+6BZp6G7niu735Sk7lN" crossorigin="anonymous"></script>
<script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.0/dist/js/bootstrap.min.js" integrity="sha384-+YQ4JLhjyBLPDQt//I+STsc9iw4uQqACwlvpslubQzn4u2UU2UFM80nGisd026JF" crossorigin="anonymous"></script>
<script src="_content/Blazorise/blazorise.js"></script>
<script src="_content/Blazorise.Bootstrap/blazorise.bootstrap.js"></script>
<script src="_content/Blazorise.DataGrid/blazorise.datagrid.js"></script>
```

## Basic Usage
see `Test` project
