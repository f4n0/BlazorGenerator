using System.Reflection;
using BlazorEngine.Components.Base;
using BlazorEngine.Security;
using BlazorEngine.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorEngine.TestHelper;

/// <summary>
/// Builds and injects service stubs into BlazorEngine components for unit testing.
/// All base services (<see cref="NavigationManager"/>, <see cref="UIServices"/>,
/// <see cref="IJSRuntime"/>, <see cref="ISecurity"/>) are pre-configured with no-op stubs.
/// Custom services can be registered via <see cref="Register{TService}(TService)"/>.
/// </summary>
public sealed class TestServices
{
  private readonly Dictionary<Type, object> _services = new();
  private readonly TestRenderer _renderer = new();

  public TestServices()
  {
    Register<NavigationManager>(new StubNavigationManager());
    Register<IJSRuntime>(NullProxy.Create<IJSRuntime>());
    Register<ISecurity>(new StubSecurity());
    var dialogStub = StubDialogService.Create();
    Register<IDialogService>((IDialogService)(object)dialogStub);
    Register(dialogStub);
    Register<IKeyCodeService>(NullProxy.Create<IKeyCodeService>());
    Register(new BlazorEngineLogger());
    Register(new ProgressService());
    Register(new LockUIService());
    Register(new UIServices(
      Get<BlazorEngineLogger>(),
      Get<IDialogService>(),
      Get<ProgressService>(),
      Get<IKeyCodeService>(),
      Get<LockUIService>()));
  }

  /// <summary>
  /// Registers a service instance. Replaces any existing registration for <typeparamref name="TService"/>.
  /// </summary>
  public TestServices Register<TService>(TService instance) where TService : class
  {
    _services[typeof(TService)] = instance;
    return this;
  }

  /// <summary>
  /// Registers a no-op stub for an interface using <see cref="NullProxy"/>.
  /// Useful for custom injected interfaces that don't need real behaviour in tests.
  /// Only works with interfaces; for concrete classes use <see cref="Register{TService}(TService)"/> instead.
  /// </summary>
  public TestServices RegisterStub<TService>() where TService : class
  {
    if (!typeof(TService).IsInterface)
      throw new InvalidOperationException(
        $"RegisterStub<{typeof(TService).Name}>() only supports interfaces. " +
        $"'{typeof(TService).Name}' is a concrete class — use Register<{typeof(TService).Name}>(new ...) instead.");
    return Register(NullProxy.Create<TService>());
  }

  /// <summary>Retrieves a previously registered service.</summary>
  public TService Get<TService>() where TService : class
  {
    if (_services.TryGetValue(typeof(TService), out var svc))
      return (TService)svc;
    throw new InvalidOperationException(
      $"Service '{typeof(TService).FullName}' is not registered. Call Register<{typeof(TService).Name}>() first.");
  }

  /// <summary>
  /// Sets every <c>[Inject]</c> property on <paramref name="component"/>,
  /// including the internal <c>Security</c> service and any custom-registered services
  /// on derived component types.
  /// </summary>
  public void InjectInto(BlazorEngineComponentBase component)
  {
    component.NavManager = Get<NavigationManager>();
    component.UIServices = Get<UIServices>();
    component.JSRuntime = Get<IJSRuntime>();

    InjectSecurity(component);
    InjectCustomProperties(component);
    SuppressRendering(component);
  }

  private void InjectSecurity(BlazorEngineComponentBase component)
  {
    var blazorEngineAssembly = typeof(BlazorEngineComponentBase).Assembly;
    var securityServiceType = blazorEngineAssembly
      .GetType("BlazorEngine.Security.BlazorEngineSecurityService")!;

    IServiceProvider serviceProvider = new StubServiceProvider(Get<ISecurity>());
    var securityService = Activator.CreateInstance(securityServiceType, [serviceProvider]);

    typeof(BlazorEngineComponentBase)
      .GetProperty("Security", BindingFlags.NonPublic | BindingFlags.Instance)!
      .SetValue(component, securityService);
  }

  // Attaches the component to a no-op renderer so that StateHasChanged() and
  // InvokeAsync() never throw "The render handle is not yet assigned".
  // The _renderFragment is replaced with a no-op to prevent BuildRenderTree
  // from being called during tests.
  private static readonly FieldInfo RenderFragmentField =
    typeof(ComponentBase).GetField("_renderFragment", BindingFlags.NonPublic | BindingFlags.Instance)!;

  private void SuppressRendering(BlazorEngineComponentBase component)
  {
    _renderer.Attach(component);
    RenderFragmentField.SetValue(component, (RenderFragment)(_ => { }));
  }

  private void InjectCustomProperties(BlazorEngineComponentBase component)
  {
    var baseType = typeof(BlazorEngineComponentBase);
    var current = component.GetType();

    while (current is not null && current != baseType && current != typeof(object))
    {
      foreach (var prop in current.GetProperties(
                 BindingFlags.Public | BindingFlags.NonPublic |
                 BindingFlags.Instance | BindingFlags.DeclaredOnly))
      {
        if (!prop.CanWrite) continue;
        if (prop.GetCustomAttribute<InjectAttribute>() is null) continue;

        if (_services.TryGetValue(prop.PropertyType, out var service))
          prop.SetValue(component, service);
      }

      current = current.BaseType;
    }
  }
}
