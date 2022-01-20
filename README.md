# BlazorGenerator
It will autogenerate at runtime blazor pages according to attributes and properties

## Getting Started
Edit `_Imports.razor` and add: 
```
@using BlazorGenerator.Components
@using BlazorGenerator.Services
```
Edit `App.razor`:
```
<Router AppAssembly="@typeof(App).Assembly">
    <Found Context="routeData">
        <RouteView RouteData="@routeData" DefaultLayout="@typeof(DynamicMainLayout)" />
        <FocusOnNavigate RouteData="@routeData" Selector="h1" />
    </Found>
    <NotFound>
        <PageTitle>Not found</PageTitle>
        <LayoutView Layout="@typeof(DynamicMainLayout)">
            <p role="alert">Sorry, there's nothing at this address.</p>
        </LayoutView>
    </NotFound>
</Router>
```
    
In `Program.cs` add BlazorGen Services:
```
using BlazorGenerator;
builder.Services.AddBlazorGen();
```

BlazorGen use Blazorise for its components, so edit the `_Layout_.cshtml` adding:
#### Before body closing:    
```
	<!-- inside of body section and after the div/app tag  -->
	<script src="https://cdn.jsdelivr.net/npm/jquery@3.5.1/dist/jquery.slim.min.js" integrity="sha384-DfXdz2htPH0lsSSs5nCTpuj/zy4C+OGpamoFVy38MVBnE+IbbVYUew+OrCXaRkfj" crossorigin="anonymous"></script>
	<script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.min.js" integrity="sha384-9/reFTGAW83EW2RDu2S0VKaIzap3H66lZH81PoYlFhbGU+6BZp6G7niu735Sk7lN" crossorigin="anonymous"></script>
	<script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.1/dist/js/bootstrap.min.js" integrity="sha384-VHvPCCyXqtD5DqJeNxl2dtTyhF78xXNXdkwX1CZeRusQfRKp+tA7hAShOK/B/fQ2" crossorigin="anonymous"></script>

```

# Available Pages type:
- CardPage
- ListPage
- WorksheetPage

To add the page in the menu, use the class' attribute `AddToMenu`

## Others:
### Modal Pages:
to use a modal (available in every pages):
1. Call InitModal where ModalType is the model to use (the model must implement `ModalPage`) and ModalData the data to show
2. Call OpenModal
3. If you need to save the data eventually edited in the modal, override the trigger `OnModalSave`

### Actions
The actions are available in all pages (excluded modals)
To create the action create a void method and add the attribute `PageAction`
## Basic Usage
see `Test` project
