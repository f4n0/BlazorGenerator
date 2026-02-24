using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Mono.Cecil;

namespace BlazorEngine.BuildTasks
{
  /// <summary>
  /// MSBuild task that inspects a compiled assembly using Mono.Cecil (no AppDomain loading)
  /// and extracts BlazorEngine attribute metadata into a compact binary file.
  /// This runs at build time, eliminating the need for runtime assembly scanning.
  /// </summary>
  public class ExtractBlazorMetadataTask : Task
  {
    [Required]
    public string AssemblyPath { get; set; } = "";

    [Required]
    public string OutputPath { get; set; } = "";

    [Output]
    public int MenuItemCount { get; set; }

    [Output]
    public int FooterLinkCount { get; set; }

    [Output]
    public int PageActionCount { get; set; }

    [Output]
    public int GridActionCount { get; set; }

    [Output]
    public int ContextMenuCount { get; set; }

    // Attribute full names we're scanning for
    private const string AddToMenuAttr = "BlazorEngine.Attributes.AddToMenuAttribute";
    private const string FooterLinkAttr = "BlazorEngine.Attributes.FooterLinkAttribute";
    private const string PageActionAttr = "BlazorEngine.Attributes.PageActionAttribute";
    private const string GridActionAttr = "BlazorEngine.Attributes.GridActionAttribute";
    private const string ContextMenuAttr = "BlazorEngine.Attributes.ContextMenuAttribute";

    public override bool Execute()
    {
      if (!File.Exists(AssemblyPath))
      {
        Log.LogWarning("BlazorEngine: Assembly not found at '{0}', skipping metadata extraction.", AssemblyPath);
        return true; // Not a failure — assembly may not exist yet in incremental builds
      }

      try
      {
        var metadata = ExtractMetadata();

        MenuItemCount = metadata.MenuItems.Count;
        FooterLinkCount = metadata.FooterLinks.Count;
        PageActionCount = metadata.PageActions.Sum(t => t.Actions.Count);
        GridActionCount = metadata.GridActions.Sum(t => t.Actions.Count);
        ContextMenuCount = metadata.ContextMenus.Sum(t => t.Actions.Count);

        var totalItems = MenuItemCount + FooterLinkCount + PageActionCount + GridActionCount + ContextMenuCount;

        if (totalItems == 0)
        {
          // No BlazorEngine attributes found — skip writing file
          Log.LogMessage(MessageImportance.Low,
            "BlazorEngine: No attributes found in '{0}', skipping metadata file.", Path.GetFileName(AssemblyPath));

          // Clean up old metadata file if it exists
          if (File.Exists(OutputPath))
            File.Delete(OutputPath);

          return true;
        }

        // Ensure output directory exists
        var dir = Path.GetDirectoryName(OutputPath);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
          Directory.CreateDirectory(dir);

        using (var stream = File.Create(OutputPath))
        {
          MetadataFormat.Write(stream, metadata);
        }

        Log.LogMessage(MessageImportance.Normal,
          "BlazorEngine: Extracted metadata from '{0}' — {1} menu items, {2} footer links, {3} page actions, {4} grid actions, {5} context menus.",
          Path.GetFileName(AssemblyPath), MenuItemCount, FooterLinkCount, PageActionCount, GridActionCount, ContextMenuCount);

        return true;
      }
      catch (Exception ex)
      {
        Log.LogWarning("BlazorEngine: Failed to extract metadata from '{0}': {1}", AssemblyPath, ex.Message);
        return true; // Degrade gracefully — runtime reflection will be used as fallback
      }
    }

    private MetadataFormat.AssemblyMetadataFile ExtractMetadata()
    {
      var metadata = new MetadataFormat.AssemblyMetadataFile();

      // Read assembly without locking it, and without loading dependencies
      var readerParams = new ReaderParameters
      {
        ReadingMode = ReadingMode.Deferred,
        ReadSymbols = false
      };

      using (var assembly = AssemblyDefinition.ReadAssembly(AssemblyPath, readerParams))
      {
        foreach (var type in assembly.MainModule.Types)
        {
          ProcessType(type, metadata);

          // Handle nested types
          if (type.HasNestedTypes)
          {
            foreach (var nested in type.NestedTypes)
              ProcessType(nested, metadata);
          }
        }
      }

      return metadata;
    }

    private void ProcessType(TypeDefinition type, MetadataFormat.AssemblyMetadataFile metadata)
    {
      if (!type.HasCustomAttributes && !type.HasMethods)
        return;

      // Check class-level attributes
      if (type.HasCustomAttributes)
      {
        foreach (var attr in type.CustomAttributes)
        {
          var attrName = attr.AttributeType.FullName;

          if (attrName == AddToMenuAttr)
          {
            metadata.MenuItems.Add(ExtractMenuItemEntry(type, attr));
          }
          else if (attrName == FooterLinkAttr)
          {
            metadata.FooterLinks.Add(ExtractFooterLinkEntry(type, attr));
          }
        }
      }

      // Check method-level attributes
      if (type.HasMethods)
      {
        var pageActions = new List<MetadataFormat.ActionEntry>();
        var gridActions = new List<MetadataFormat.ActionEntry>();
        var contextMenus = new List<MetadataFormat.ActionEntry>();

        foreach (var method in type.Methods)
        {
          if (!method.HasCustomAttributes || method.IsSpecialName)
            continue;

          foreach (var attr in method.CustomAttributes)
          {
            var attrName = attr.AttributeType.FullName;

            if (attrName == PageActionAttr)
            {
              pageActions.Add(ExtractPageActionEntry(method, attr));
            }
            else if (attrName == GridActionAttr)
            {
              gridActions.Add(ExtractGridActionEntry(method, attr));
            }
            else if (attrName == ContextMenuAttr)
            {
              contextMenus.Add(ExtractContextMenuEntry(method, attr));
            }
          }
        }

        var typeFullName = type.FullName;

        if (pageActions.Count > 0)
        {
          metadata.PageActions.Add(new MetadataFormat.TypeActions
          {
            TypeFullName = typeFullName,
            Actions = pageActions
          });
        }

        if (gridActions.Count > 0)
        {
          metadata.GridActions.Add(new MetadataFormat.TypeActions
          {
            TypeFullName = typeFullName,
            Actions = gridActions
          });
        }

        if (contextMenus.Count > 0)
        {
          metadata.ContextMenus.Add(new MetadataFormat.TypeActions
          {
            TypeFullName = typeFullName,
            Actions = contextMenus
          });
        }
      }
    }

    private MetadataFormat.MenuItemEntry ExtractMenuItemEntry(TypeDefinition type, CustomAttribute attr)
    {
      var entry = new MetadataFormat.MenuItemEntry
      {
        TypeFullName = type.FullName
      };

      foreach (var prop in attr.Properties)
      {
        switch (prop.Name)
        {
          case "Title":
            entry.Title = (string)prop.Argument.Value;
            break;
          case "Route":
            entry.Route = (string)prop.Argument.Value;
            break;
          case "Icon":
            entry.IconTypeName = GetTypeRefFullName(prop.Argument);
            break;
          case "Group":
            entry.Group = (string)prop.Argument.Value;
            break;
          case "OrderSequence":
            entry.OrderSequence = (int)prop.Argument.Value;
            break;
        }
      }

      return entry;
    }

    private MetadataFormat.FooterLinkEntry ExtractFooterLinkEntry(TypeDefinition type, CustomAttribute attr)
    {
      var entry = new MetadataFormat.FooterLinkEntry
      {
        TypeFullName = type.FullName
      };

      foreach (var prop in attr.Properties)
      {
        switch (prop.Name)
        {
          case "Title":
            entry.Title = (string)prop.Argument.Value;
            break;
          case "Route":
            entry.Route = (string)prop.Argument.Value;
            break;
          case "Icon":
            entry.IconTypeName = GetTypeRefFullName(prop.Argument);
            break;
          case "OpenNewWindow":
            entry.OpenNewWindow = (bool)prop.Argument.Value;
            break;
        }
      }

      return entry;
    }

    private MetadataFormat.ActionEntry ExtractPageActionEntry(MethodDefinition method, CustomAttribute attr)
    {
      var entry = new MetadataFormat.ActionEntry
      {
        MethodName = method.Name
      };

      foreach (var prop in attr.Properties)
      {
        switch (prop.Name)
        {
          case "Caption":
            entry.Caption = (string)prop.Argument.Value;
            break;
          case "Group":
            entry.Group = (string)prop.Argument.Value;
            break;
          case "Icon":
            entry.IconTypeName = GetTypeRefFullName(prop.Argument);
            break;
        }
      }

      return entry;
    }

    private MetadataFormat.ActionEntry ExtractGridActionEntry(MethodDefinition method, CustomAttribute attr)
    {
      var entry = new MetadataFormat.ActionEntry
      {
        MethodName = method.Name
      };

      foreach (var prop in attr.Properties)
      {
        switch (prop.Name)
        {
          case "Caption":
            entry.Caption = (string)prop.Argument.Value;
            break;
          case "GridIcon":
            entry.IconTypeName = GetTypeRefFullName(prop.Argument);
            break;
        }
      }

      return entry;
    }

    private MetadataFormat.ActionEntry ExtractContextMenuEntry(MethodDefinition method, CustomAttribute attr)
    {
      var entry = new MetadataFormat.ActionEntry
      {
        MethodName = method.Name
      };

      foreach (var prop in attr.Properties)
      {
        switch (prop.Name)
        {
          case "Caption":
            entry.Caption = (string)prop.Argument.Value;
            break;
          case "Icon":
            entry.IconTypeName = GetTypeRefFullName(prop.Argument);
            break;
        }
      }

      return entry;
    }

    private static string GetTypeRefFullName(CustomAttributeArgument argument)
    {
      if (argument.Value is TypeReference typeRef)
      {
        // Return assembly-qualified-ish name: "Namespace.TypeName, AssemblyName"
        return typeRef.FullName + ", " + typeRef.Scope.Name;
      }

      return argument.Value?.ToString() ?? "";
    }
  }
}
