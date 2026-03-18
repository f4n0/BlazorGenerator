using System.Reflection;
using BlazorEngine.Components.Base;

namespace BlazorEngine.TestHelper;

/// <summary>
/// Provides utilities for unit-testing BlazorEngine pages (CardPage, ListPage, Worksheet, TwoList)
/// without requiring a running Blazor host or DI container.
/// Protected/internal lifecycle methods are invoked via reflection.
/// </summary>
public static class Instantiator<T> where T : BlazorEngineComponentBase
{
  private const BindingFlags NonPublicInstance =
    BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;

  /// <summary>Creates an instance of <typeparamref name="T"/> without a DI container.</summary>
  public static T Create() => Activator.CreateInstance<T>();

  /// <summary>
  /// Creates an instance of <typeparamref name="T"/> and injects all services from <paramref name="services"/>.
  /// </summary>
  public static T Create(TestServices services)
  {
    var instance = Activator.CreateInstance<T>();
    services.InjectInto(instance);
    return instance;
  }

  /// <summary>Invokes the protected <c>LoadVisibleFields</c> override on <paramref name="instance"/>.</summary>
  public static async Task LoadVisibleFieldsAsync(T instance)
  {
    var method = FindMethod(typeof(T), "LoadVisibleFields");
    await ((Task)method.Invoke(instance, null)!).ConfigureAwait(false);
  }

  /// <summary>Invokes the protected <c>LoadData</c> override on <paramref name="instance"/>.</summary>
  public static async Task LoadDataAsync(T instance)
  {
    var method = FindMethod(typeof(T), "LoadData");
    await ((Task)method.Invoke(instance, null)!).ConfigureAwait(false);
  }

  /// <summary>
  /// Creates an instance of <typeparamref name="T"/>, then calls
  /// <c>LoadVisibleFields</c> followed by <c>LoadData</c> in order.
  /// </summary>
  public static async Task<T> CreateAndLoadAsync()
  {
    var instance = Create();
    await LoadVisibleFieldsAsync(instance).ConfigureAwait(false);
    await LoadDataAsync(instance).ConfigureAwait(false);
    return instance;
  }

  /// <summary>
  /// Creates an instance with injected <paramref name="services"/>, then calls
  /// <c>LoadVisibleFields</c> followed by <c>LoadData</c>.
  /// </summary>
  public static async Task<T> CreateAndLoadAsync(TestServices services)
  {
    var instance = Create(services);
    await LoadVisibleFieldsAsync(instance).ConfigureAwait(false);
    await LoadDataAsync(instance).ConfigureAwait(false);
    return instance;
  }

  private static MethodInfo FindMethod(Type type, string methodName)
  {
    var current = type;
    while (current is not null && current != typeof(object))
    {
      var method = current.GetMethod(methodName, NonPublicInstance);
      if (method is not null) return method;
      current = current.BaseType;
    }

    throw new InvalidOperationException(
      $"Method '{methodName}' not found on type '{type.FullName}'.");
  }
}
