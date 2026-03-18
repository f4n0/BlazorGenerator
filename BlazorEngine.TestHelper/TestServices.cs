using System.Reflection;
using BlazorEngine.Components.Base;
using BlazorEngine.Models;
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

  public TestServices()
  {
    Register<NavigationManager>(new StubNavigationManager());
    Register<IJSRuntime>(NullProxy.Create<IJSRuntime>());
    Register<ISecurity>(new StubSecurity());
    Register<IDialogService>(NullProxy.Create<IDialogService>());
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
  /// </summary>
  public TestServices RegisterStub<TService>() where TService : class
    => Register(NullProxy.Create<TService>());

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

// ---------------------------------------------------------------------------
// Stub implementations
// ---------------------------------------------------------------------------

/// <summary>Minimal <see cref="NavigationManager"/> stub that records the last navigation.</summary>
public sealed class StubNavigationManager : NavigationManager
{
  /// <summary>The last URI passed to <see cref="NavigationManager.NavigateTo(string, bool)"/>.</summary>
  public string? LastNavigatedUri { get; private set; }

  public StubNavigationManager(string baseUri = "https://localhost/")
  {
    Initialize(baseUri, baseUri);
  }

  protected override void NavigateToCore(string uri, NavigationOptions options)
  {
    LastNavigatedUri = uri;
  }
}

/// <summary><see cref="ISecurity"/> stub that grants full permissions.</summary>
public sealed class StubSecurity : ISecurity
{
  public Task<PermissionSet> GetPermissionSet(Type? type = null)
    => Task.FromResult(new PermissionSet
    {
      Object = type,
      Insert = true,
      Delete = true,
      Modify = true,
      Execute = true
    });

  public Task<string> GetCurrentSessionIdentifier()
    => Task.FromResult("test-session");
}

/// <summary>
/// Minimal <see cref="IServiceProvider"/> that resolves only an <see cref="ISecurity"/> instance.
/// Used internally to construct the <c>BlazorEngineSecurityService</c>.
/// </summary>
internal sealed class StubServiceProvider(ISecurity security) : IServiceProvider
{
  public object? GetService(Type serviceType)
    => serviceType == typeof(ISecurity) ? security : null;
}

/// <summary>
/// <see cref="DispatchProxy"/>-based stub that intercepts every method call on an interface
/// and returns a safe default (<c>Task.CompletedTask</c>, <c>default(T)</c>, etc.).
/// <para>
/// Use <see cref="Create{T}"/> to produce a stub for any interface without writing a manual implementation.
/// </para>
/// </summary>
public class NullProxy : DispatchProxy
{
  private static readonly MethodInfo TaskFromResultMethod =
    typeof(Task).GetMethod(nameof(Task.FromResult))!;

  /// <summary>Creates a no-op proxy that implements <typeparamref name="T"/>.</summary>
  public static T Create<T>() where T : class
    => DispatchProxy.Create<T, NullProxy>();

  /// <inheritdoc />
  protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
  {
    if (targetMethod is null) return null;
    var rt = targetMethod.ReturnType;

    if (rt == typeof(void)) return null;
    if (rt == typeof(Task)) return Task.CompletedTask;
    if (rt == typeof(ValueTask)) return ValueTask.CompletedTask;

    if (rt.IsGenericType)
    {
      var gtd = rt.GetGenericTypeDefinition();
      var inner = rt.GetGenericArguments()[0];

      if (gtd == typeof(Task<>))
        return TaskFromResultMethod
          .MakeGenericMethod(inner)
          .Invoke(null, [DefaultOf(inner)]);

      if (gtd == typeof(ValueTask<>))
      {
        var task = TaskFromResultMethod
          .MakeGenericMethod(inner)
          .Invoke(null, [DefaultOf(inner)]);
        return Activator.CreateInstance(rt, [task]);
      }
    }

    return DefaultOf(rt);
  }

  private static object? DefaultOf(Type type)
    => type.IsValueType ? Activator.CreateInstance(type) : null;
}
