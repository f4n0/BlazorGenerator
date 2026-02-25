using BlazorEngine.Services;

namespace BlazorEngine.Components.Navigation;

public partial class MetadataViewer
{
  private IReadOnlyList<MetadataRegistry.AssemblyRegistrationInfo> _registrations = [];

  protected override void OnInitialized()
  {
    _registrations = MetadataRegistry.GetRegistrationInfo();
  }
}