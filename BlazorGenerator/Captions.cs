namespace BlazorGenerator
{
  public class Captions
  {
    private static Captions? _instance;
    public static Captions Instance => _instance ??= new Captions();

    public string Save { get; set; } = "Save";
    public string Discard { get; set; } = "Discard";
    public string Cancel { get; set; } = "Cancel";
    public string Search { get; set; } = "Search";
    public string LockUIMessage { get; set; } = "Another operation is currently running, please wait";
    public string NoLog { get; set; } = "No Logs found";
    public string Home { get; set; } = "Home";
    public string FileInputMessage { get; set; } = "Drag files here you wish to upload, or";
    public string Browse { get; set; } = "Browse";
    public string NavigationBack { get; set; } = "Back";

    public string NotFound { get; set; } = "Not Found";
    public string NotFoundMessage { get; set; } = "Sorry, there's nothing at this address.";

    public string ErrorTitle { get; set; } = "Oh, An error occurred!";
    public string ErrorMessage { get; set; } = "Click refresh and start again!";
    public string ErrorMessagePersist { get; set; } = "The error persists? Click go to home page";
    public string ErrorCopyDetails { get; set; } = "copy details";
    public string ErrorRefresh { get; set; } = "Refresh Page";
    public string ErrorHomePage { get; set; } = "Home Page";

    public string ToggleDarkTheme { get; set; } = "Toggle Dark Theme";

    public string ExportExcel { get; set; } = "Export to Excel";
  }
}
