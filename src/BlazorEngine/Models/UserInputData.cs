namespace BlazorEngine.Models;

public class UserInputData
{
  public string Message { get; set; } = string.Empty;
  public string Result { get; set; } = string.Empty;
  public UserInputType InputType { get; set; } = UserInputType.PlainText;
  public List<string> Choices { get; set; } = [];
}