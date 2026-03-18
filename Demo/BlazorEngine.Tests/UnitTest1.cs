using BlazorEngine.Services;
using BlazorEngine.TestHelper;
using Microsoft.AspNetCore.Components;
using TestShared.Views;

namespace BlazorEngine.Tests
{
  public class UnitTest1
  {
    [Fact]
    public async Task Test1Async()
    {
      var services = new TestServices();
      CardView card = await Instantiator<CardView>.CreateAndLoadAsync(services);
      Assert.NotEmpty(card.VisibleFields);
      Assert.NotNull(card.Content);

      card.GoTo();
    }
  }
}
