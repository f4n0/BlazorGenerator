using System.Reflection;
using BlazorEngine.Models;
using BlazorEngine.Security;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorEngine.TestHelper;

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
/// Configurable <see cref="IDialogService"/> stub backed by <see cref="DispatchProxy"/>.
/// All dialog-show methods return a <see cref="StubDialogReference"/> whose result defaults
/// to <see cref="DialogResult.Cancel()"/> (user dismissed). Set <see cref="NextResult"/>
/// before calling a page method to control what the dialog returns.
/// </summary>
public class StubDialogService : DispatchProxy
{
  private static readonly MethodInfo TaskFromResultMethod =
    typeof(Task).GetMethod(nameof(Task.FromResult))!;

  /// <summary>
  /// The <see cref="DialogResult"/> that the next dialog will return.
  /// Defaults to <see cref="DialogResult.Cancel()"/> (user dismissed the dialog).
  /// Set to <c>DialogResult.Ok(myData)</c> to simulate a successful dialog.
  /// </summary>
  public DialogResult NextResult { get; set; } = DialogResult.Cancel();

  /// <summary>Creates a new configurable stub that also implements <see cref="IDialogService"/>.</summary>
  public static StubDialogService Create()
    => (StubDialogService)(object)DispatchProxy.Create<IDialogService, StubDialogService>();

  /// <inheritdoc />
  protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
  {
    if (targetMethod is null) return null;
    var rt = targetMethod.ReturnType;

    // All ShowDialogAsync / ShowPanelAsync / ShowSplashScreenAsync overloads
    // return Task<IDialogReference> — give them a real reference.
    if (rt == typeof(Task<IDialogReference>))
    {
      IDialogReference reference = new StubDialogReference(NextResult);
      return Task.FromResult(reference);
    }

    // Fallback: NullProxy-like behaviour for everything else
    if (rt == typeof(void)) return null;
    if (rt == typeof(Task)) return Task.CompletedTask;
    if (rt == typeof(ValueTask)) return ValueTask.CompletedTask;

    if (rt.IsGenericType)
    {
      var gtd = rt.GetGenericTypeDefinition();
      var inner = rt.GetGenericArguments()[0];

      if (gtd == typeof(Task<>))
        return TaskFromResultMethod.MakeGenericMethod(inner)
          .Invoke(null, [inner.IsValueType ? Activator.CreateInstance(inner) : null]);

      if (gtd == typeof(ValueTask<>))
      {
        var task = TaskFromResultMethod.MakeGenericMethod(inner)
          .Invoke(null, [inner.IsValueType ? Activator.CreateInstance(inner) : null]);
        return Activator.CreateInstance(rt, [task]);
      }
    }

    return rt.IsValueType ? Activator.CreateInstance(rt) : null;
  }
}

/// <summary>
/// Minimal <see cref="IDialogReference"/> stub whose <see cref="Result"/> is pre-configured
/// at construction time. Returned by <see cref="StubDialogService"/>.
/// </summary>
public sealed class StubDialogReference : IDialogReference
{
  /// <summary>Creates a reference that will resolve with the given <paramref name="result"/>.</summary>
  public StubDialogReference(DialogResult? result = null)
  {
    Result = Task.FromResult(result ?? DialogResult.Cancel());
  }

  /// <inheritdoc />
  public string Id { get; } = Guid.NewGuid().ToString();

  /// <inheritdoc />
  public Task<DialogResult> Result { get; set; }

  /// <inheritdoc />
  public DialogInstance? Instance { get; set; }

  /// <inheritdoc />
  public Task CloseAsync() => Task.CompletedTask;

  /// <inheritdoc />
  public Task CloseAsync(DialogResult result) => Task.CompletedTask;

  /// <inheritdoc />
  public bool Dismiss(DialogResult result) => true;

  /// <inheritdoc />
  public Task<TResult?> GetReturnValueAsync<TResult>() => Task.FromResult(default(TResult));
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
