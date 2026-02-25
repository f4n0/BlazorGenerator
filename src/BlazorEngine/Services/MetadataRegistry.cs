using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using BlazorEngine.Attributes;

namespace BlazorEngine.Services;

/// <summary>
///   Central registry for BlazorEngine type metadata.
///   Populated automatically by module initializers at assembly load time
///   (from embedded binary metadata) or lazily via reflection fallback.
///   This replaces the runtime assembly scanning in AttributesUtils with
///   pre-computed data extracted at build time.
/// </summary>
public static class MetadataRegistry
{
  public enum RegistrationSource
  {
    BuildTime,
    ReflectionFallback
  }
  // ── Resolved runtime data ───────────────────────────────────────────

  private static readonly ConcurrentBag<(Type Type, AddToMenuAttribute Attribute)> _menuItems = new();
  private static readonly ConcurrentBag<(Type Type, FooterLinkAttribute Attribute)> _footerLinks = new();

  private static readonly ConcurrentDictionary<Type, (MethodInfo Method, PageActionAttribute Attribute)[]>
    _pageActions = new();

  private static readonly ConcurrentDictionary<Type, (MethodInfo Method, GridActionAttribute Attribute)[]>
    _gridActions = new();

  private static readonly ConcurrentDictionary<Type, (MethodInfo Method, ContextMenuAttribute Attribute)[]>
    _contextMenus = new();

  // ── Assembly tracking ───────────────────────────────────────────────

  private static readonly ConcurrentDictionary<string, AssemblyRegistration> _registrations = new();
  private static readonly ConcurrentBag<Assembly> _fallbackAssemblies = new();
  private static volatile bool _fallbackResolved;
  private static readonly object _fallbackLock = new();

  /// <summary>
  ///   Tracks whether any metadata was registered (build-time or fallback).
  /// </summary>
  public static bool HasRegistrations => !_registrations.IsEmpty || !_fallbackAssemblies.IsEmpty;

  /// <summary>
  ///   Diagnostics: number of assemblies with pre-computed metadata.
  /// </summary>
  public static int PreComputedAssemblyCount =>
    _registrations.Count(r => r.Value.Source == RegistrationSource.BuildTime);

  /// <summary>
  ///   Diagnostics: number of assemblies using reflection fallback.
  /// </summary>
  public static int FallbackAssemblyCount => _fallbackAssemblies.Count;

  // ── Registration API (called from module initializers) ──────────────

  /// <summary>
  ///   Register an assembly's metadata from an embedded binary resource stream.
  ///   Called automatically by the generated module initializer at assembly load time.
  /// </summary>
  public static void RegisterFromStream(Assembly assembly, Stream stream)
  {
    var assemblyName = assembly.GetName().Name ?? assembly.FullName ?? "unknown";

    try
    {
      var metadata = BinaryMetadataReader.Read(stream);
      stream.Dispose();

      var registration = new AssemblyRegistration
      {
        Assembly = assembly,
        Source = RegistrationSource.BuildTime,
        RawMetadata = metadata
      };

      if (_registrations.TryAdd(assemblyName, registration))
      {
        // First registration — resolve build-time metadata
        ResolveMetadata(assembly, metadata);
      }
      else if (_registrations.TryGetValue(assemblyName, out var existing)
               && existing.Source == RegistrationSource.ReflectionFallback)
      {
        // Fallback was registered first (race with EnsureFallbackResolved).
        // Upgrade to build-time: remove stale reflection data, replace registration.
        RemoveFallbackData(assembly);

        var upgraded = new AssemblyRegistration
        {
          Assembly = assembly,
          Source = RegistrationSource.BuildTime,
          RawMetadata = metadata
        };
        _registrations[assemblyName] = upgraded;

        ResolveMetadata(assembly, metadata);
      }
    }
    catch (Exception ex)
    {
      // Build-time data is corrupted or incompatible — register for fallback
      Debug.WriteLine(
        $"BlazorEngine: Failed to load metadata for '{assemblyName}': {ex.Message}. Using reflection fallback.");
      RegisterAssemblyForFallback(assembly);
    }
  }

  /// <summary>
  ///   Register an assembly for lazy reflection-based discovery.
  ///   Used when no embedded metadata is available (first build, or opt-out).
  /// </summary>
  public static void RegisterAssemblyForFallback(Assembly assembly)
  {
    _fallbackAssemblies.Add(assembly);
  }

  // ── Query API (replaces AttributesUtils calls) ──────────────────────

  /// <summary>
  ///   Get all menu items from all registered assemblies.
  ///   Replaces AttributesUtils.GetModelsWithAttribute&lt;AddToMenuAttribute&gt;().
  /// </summary>
  public static IReadOnlyList<(Type Type, AddToMenuAttribute Attribute)> GetMenuItems()
  {
    EnsureFallbackResolved();
    return _menuItems.ToArray();
  }

  /// <summary>
  ///   Get all footer links from all registered assemblies.
  /// </summary>
  public static IReadOnlyList<(Type Type, FooterLinkAttribute Attribute)> GetFooterLinks()
  {
    EnsureFallbackResolved();
    return _footerLinks.ToArray();
  }

  /// <summary>
  ///   Get page actions for a specific type.
  ///   Replaces AttributesUtils.GetMethodsWithAttribute&lt;PageActionAttribute&gt;(obj).
  /// </summary>
  public static IEnumerable<(MethodInfo Method, PageActionAttribute Attribute)> GetPageActions(object obj)
  {
    EnsureFallbackResolved();
    var type = obj.GetType();
    return _pageActions.GetOrAdd(type, static t => DiscoverMethodAttribute<PageActionAttribute>(t));
  }

  /// <summary>
  ///   Get grid actions for a specific type.
  ///   Replaces AttributesUtils.GetMethodsWithAttribute&lt;GridActionAttribute&gt;(obj).
  /// </summary>
  public static IEnumerable<(MethodInfo Method, GridActionAttribute Attribute)> GetGridActions(object obj)
  {
    EnsureFallbackResolved();
    var type = obj.GetType();
    return _gridActions.GetOrAdd(type, static t => DiscoverMethodAttribute<GridActionAttribute>(t));
  }

  /// <summary>
  ///   Get context menu actions for a specific type.
  /// </summary>
  public static IEnumerable<(MethodInfo Method, ContextMenuAttribute Attribute)> GetContextMenuItems(object obj)
  {
    EnsureFallbackResolved();
    var type = obj.GetType();
    return _contextMenus.GetOrAdd(type, static t => DiscoverMethodAttribute<ContextMenuAttribute>(t));
  }

  // ── Diagnostics ─────────────────────────────────────────────────────

  /// <summary>
  ///   Get diagnostic information about all registered assemblies.
  /// </summary>
  public static IReadOnlyList<AssemblyRegistrationInfo> GetRegistrationInfo()
  {
    EnsureFallbackResolved();

    var result = new List<AssemblyRegistrationInfo>();

    foreach (var kvp in _registrations)
      result.Add(new AssemblyRegistrationInfo
      {
        AssemblyName = kvp.Key,
        Source = kvp.Value.Source,
        MenuItemCount = kvp.Value.RawMetadata?.MenuItems.Count ?? 0,
        FooterLinkCount = kvp.Value.RawMetadata?.FooterLinks.Count ?? 0,
        PageActionTypeCount = kvp.Value.RawMetadata?.PageActions.Count ?? 0,
        GridActionTypeCount = kvp.Value.RawMetadata?.GridActions.Count ?? 0,
        ContextMenuTypeCount = kvp.Value.RawMetadata?.ContextMenus.Count ?? 0
      });

    return result;
  }

  // ── Internal resolution ─────────────────────────────────────────────

  private static void ResolveMetadata(Assembly assembly, BinaryMetadataReader.AssemblyMetadata metadata)
  {
    // Resolve menu items
    foreach (var item in metadata.MenuItems)
    {
      var type = assembly.GetType(item.TypeFullName);
      if (type == null) continue;

      var attr = new AddToMenuAttribute
      {
        Title = item.Title,
        Route = item.Route,
        Group = item.Group,
        OrderSequence = item.OrderSequence
      };

      // Resolve icon type
      var iconType = ResolveType(item.IconTypeName, assembly);
      if (iconType != null)
        attr.Icon = iconType;

      _menuItems.Add((type, attr));
    }

    // Resolve footer links
    foreach (var item in metadata.FooterLinks)
    {
      var type = assembly.GetType(item.TypeFullName);
      if (type == null) continue;

      var attr = new FooterLinkAttribute
      {
        Title = item.Title,
        Route = item.Route,
        OpenNewWindow = item.OpenNewWindow
      };

      var iconType = ResolveType(item.IconTypeName, assembly);
      if (iconType != null)
        attr.Icon = iconType;

      _footerLinks.Add((type, attr));
    }

    // Pre-populate page actions from metadata
    foreach (var typeActions in metadata.PageActions)
    {
      var type = assembly.GetType(typeActions.TypeFullName);
      if (type == null) continue;

      var actions = ResolveActions<PageActionAttribute>(type, typeActions.Actions, assembly);
      _pageActions.TryAdd(type, actions);
    }

    // Pre-populate grid actions from metadata
    foreach (var typeActions in metadata.GridActions)
    {
      var type = assembly.GetType(typeActions.TypeFullName);
      if (type == null) continue;

      var actions = ResolveActions<GridActionAttribute>(type, typeActions.Actions, assembly);
      _gridActions.TryAdd(type, actions);
    }

    // Pre-populate context menu actions
    foreach (var typeActions in metadata.ContextMenus)
    {
      var type = assembly.GetType(typeActions.TypeFullName);
      if (type == null) continue;

      var actions = ResolveActions<ContextMenuAttribute>(type, typeActions.Actions, assembly);
      _contextMenus.TryAdd(type, actions);
    }
  }

  private static (MethodInfo Method, TAttribute Attribute)[] ResolveActions<TAttribute>(
    Type type,
    List<BinaryMetadataReader.ActionEntry> entries,
    Assembly assembly) where TAttribute : Attribute
  {
    var result = new List<(MethodInfo, TAttribute)>();
    const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    foreach (var entry in entries)
    {
      var method = type.GetMethod(entry.MethodName, flags);
      if (method == null) continue;

      TAttribute attr;

      if (typeof(TAttribute) == typeof(PageActionAttribute))
      {
        var pa = new PageActionAttribute { Caption = entry.Caption, Group = entry.Group };
        var iconType = ResolveType(entry.IconTypeName, assembly);
        if (iconType != null) pa.Icon = iconType;
        attr = (pa as TAttribute)!;
      }
      else if (typeof(TAttribute) == typeof(GridActionAttribute))
      {
        var ga = new GridActionAttribute { Caption = entry.Caption };
        var iconType = ResolveType(entry.IconTypeName, assembly);
        if (iconType != null) ga.GridIcon = iconType;
        attr = (ga as TAttribute)!;
      }
      else if (typeof(TAttribute) == typeof(ContextMenuAttribute))
      {
        var ca = new ContextMenuAttribute { Caption = entry.Caption };
        var iconType = ResolveType(entry.IconTypeName, assembly);
        if (iconType != null) ca.Icon = iconType;
        attr = (ca as TAttribute)!;
      }
      else
      {
        continue;
      }

      result.Add((method, attr));
    }

    return result.ToArray();
  }

  private static Type? ResolveType(string typeFullName, Assembly hintAssembly)
  {
    if (string.IsNullOrEmpty(typeFullName))
      return null;

    // Try direct Type.GetType (handles assembly-qualified names)
    var type = Type.GetType(typeFullName);
    if (type != null) return type;

    // Parse "Namespace.Type, AssemblyName" format from Cecil
    var commaIdx = typeFullName.IndexOf(',');
    if (commaIdx > 0)
    {
      var typeName = typeFullName.Substring(0, commaIdx).Trim();
      var assemblyName = typeFullName.Substring(commaIdx + 1).Trim();

      // Try loading the referenced assembly
      try
      {
        var asm = Assembly.Load(assemblyName);
        type = asm.GetType(typeName);
        if (type != null) return type;
      }
      catch
      {
        /* Assembly not available */
      }

      // Try hint assembly
      type = hintAssembly.GetType(typeName);
      if (type != null) return type;
    }

    // Try loaded assemblies
    foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
    {
      type = asm.GetType(typeFullName);
      if (type != null) return type;
    }

    return null;
  }

  // ── Fallback data removal (for build-time upgrade) ──────────────────

  /// <summary>
  ///   Removes previously resolved reflection-based data for an assembly
  ///   so that build-time metadata can replace it without duplicates.
  /// </summary>
  private static void RemoveFallbackData(Assembly assembly)
  {
    // Remove menu items whose type belongs to this assembly
    var menuToRemove = _menuItems.Where(m => m.Type.Assembly == assembly).ToList();
    if (menuToRemove.Count > 0)
    {
      var remaining = _menuItems.Where(m => m.Type.Assembly != assembly).ToList();
      // ConcurrentBag has no Remove — drain and re-add
      while (_menuItems.TryTake(out _))
      {
      }

      foreach (var item in remaining)
        _menuItems.Add(item);
    }

    // Remove footer links whose type belongs to this assembly
    var footerToRemove = _footerLinks.Where(f => f.Type.Assembly == assembly).ToList();
    if (footerToRemove.Count > 0)
    {
      var remaining = _footerLinks.Where(f => f.Type.Assembly != assembly).ToList();
      while (_footerLinks.TryTake(out _))
      {
      }

      foreach (var item in remaining)
        _footerLinks.Add(item);
    }

    // Remove cached page/grid/context actions for types from this assembly
    foreach (var key in _pageActions.Keys.Where(t => t.Assembly == assembly).ToList())
      _pageActions.TryRemove(key, out _);

    foreach (var key in _gridActions.Keys.Where(t => t.Assembly == assembly).ToList())
      _gridActions.TryRemove(key, out _);

    foreach (var key in _contextMenus.Keys.Where(t => t.Assembly == assembly).ToList())
      _contextMenus.TryRemove(key, out _);
  }

  // ── Fallback resolution ─────────────────────────────────────────────

  private static void EnsureFallbackResolved()
  {
    if (_fallbackResolved && _fallbackAssemblies.IsEmpty)
      return;

    lock (_fallbackLock)
    {
      if (_fallbackResolved)
        return;

      // Process fallback assemblies — prefer embedded metadata if available
      while (_fallbackAssemblies.TryTake(out var assembly))
      {
        var assemblyName = assembly.GetName().Name ?? "unknown";
        if (_registrations.ContainsKey(assemblyName))
          continue; // Already registered via build-time metadata

        // Try embedded build-time metadata first
        if (TryLoadEmbeddedMetadata(assembly, assemblyName))
          continue;

        ResolveViaReflection(assembly);

        _registrations.TryAdd(assemblyName, new AssemblyRegistration
        {
          Assembly = assembly,
          Source = RegistrationSource.ReflectionFallback
        });
      }

      // Also scan any unregistered assemblies that reference BlazorEngine
      // (for cases where module initializer didn't fire)
      ScanUnregisteredAssemblies();

      _fallbackResolved = true;
    }
  }

  private static void ScanUnregisteredAssemblies()
  {
    var skipPrefixes = new[]
      { "System.", "Microsoft.", "netstandard", "mscorlib", "WindowsBase", "Presentation", "Newtonsoft." };
    var blazorEngineName = typeof(MetadataRegistry).Assembly.GetName().Name;

    foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
    {
      var name = assembly.GetName().Name;
      if (name == null) continue;
      if (skipPrefixes.Any(prefix => name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))) continue;
      if (_registrations.ContainsKey(name)) continue;

      // Check if this assembly references BlazorEngine
      var referencesBlazorEngine = false;
      try
      {
        referencesBlazorEngine = assembly.GetReferencedAssemblies()
          .Any(r => r.Name == blazorEngineName);
      }
      catch
      {
        continue;
      }

      if (!referencesBlazorEngine) continue;

      // Prefer embedded build-time metadata over reflection
      if (TryLoadEmbeddedMetadata(assembly, name))
        continue;

      ResolveViaReflection(assembly);
      _registrations.TryAdd(name, new AssemblyRegistration
      {
        Assembly = assembly,
        Source = RegistrationSource.ReflectionFallback
      });
    }
  }

  /// <summary>
  ///   Attempts to load build-time metadata from an embedded resource in the assembly.
  ///   Returns true if the resource was found and metadata was successfully loaded.
  /// </summary>
  private static bool TryLoadEmbeddedMetadata(Assembly assembly, string assemblyName)
  {
    var resourceName = assemblyName + ".blazorengine.dat";

    try
    {
      using var stream = assembly.GetManifestResourceStream(resourceName);
      if (stream == null || stream.Length == 0)
        return false;

      var metadata = BinaryMetadataReader.Read(stream);

      var registration = new AssemblyRegistration
      {
        Assembly = assembly,
        Source = RegistrationSource.BuildTime,
        RawMetadata = metadata
      };

      if (_registrations.TryAdd(assemblyName, registration)) ResolveMetadata(assembly, metadata);

      return true;
    }
    catch
    {
      // Resource exists but is corrupted — fall through to reflection
      return false;
    }
  }

  private static void ResolveViaReflection(Assembly assembly)
  {
    try
    {
      foreach (var type in GetLoadableTypes(assembly))
      {
        // Menu items
        var menuAttr = type.GetCustomAttribute<AddToMenuAttribute>(true);
        if (menuAttr != null)
          _menuItems.Add((type, menuAttr));

        // Footer links
        var footerAttr = type.GetCustomAttribute<FooterLinkAttribute>(true);
        if (footerAttr != null)
          _footerLinks.Add((type, footerAttr));
      }
    }
    catch (Exception ex)
    {
      Debug.WriteLine(
        $"BlazorEngine: Reflection fallback failed for '{assembly.GetName().Name}': {ex.Message}");
    }
  }

  private static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
  {
    try
    {
      return assembly.GetTypes();
    }
    catch (ReflectionTypeLoadException ex)
    {
      return ex.Types.Where(t => t != null)!;
    }
    catch
    {
      return Array.Empty<Type>();
    }
  }

  /// <summary>
  ///   Discover methods with a given attribute on a type via reflection.
  ///   Used as fallback when build-time metadata doesn't cover method-level attributes.
  /// </summary>
  private static (MethodInfo Method, TAttribute Attribute)[] DiscoverMethodAttribute<TAttribute>(Type type)
    where TAttribute : Attribute
  {
    const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    var methods = type.GetMethods(flags);
    var list = new List<(MethodInfo, TAttribute)>();

    foreach (var m in methods)
    {
      if (m.IsSpecialName) continue;

      var attr = m.GetCustomAttribute<TAttribute>(true);
      if (attr != null)
        list.Add((m, attr));
    }

    return list.ToArray();
  }

  // ── Internal types ──────────────────────────────────────────────────

  private class AssemblyRegistration
  {
    public Assembly Assembly { get; set; } = null!;
    public RegistrationSource Source { get; set; }
    public BinaryMetadataReader.AssemblyMetadata? RawMetadata { get; set; }
  }

  public class AssemblyRegistrationInfo
  {
    public string AssemblyName { get; set; } = "";
    public RegistrationSource Source { get; set; }
    public int MenuItemCount { get; set; }
    public int FooterLinkCount { get; set; }
    public int PageActionTypeCount { get; set; }
    public int GridActionTypeCount { get; set; }
    public int ContextMenuTypeCount { get; set; }
  }
}