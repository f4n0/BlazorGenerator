using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BlazorEngine.BuildTasks
{
  /// <summary>
  /// Binary metadata format for BlazorEngine build-time type discovery.
  /// Compact binary serialization that replaces runtime assembly scanning.
  ///
  /// Format layout:
  /// [Header]   Magic(4 bytes) + Version(2 bytes)
  /// [Section]  MenuItems:     Count + (TypeFullName, Title, Route, IconTypeName, Group, OrderSequence)[]
  /// [Section]  FooterLinks:   Count + (TypeFullName, Title, Route, IconTypeName, OpenNewWindow)[]
  /// [Section]  PageActions:   TypeCount + (TypeFullName, MethodCount + (MethodName, Caption, Group, IconTypeName)[])[]
  /// [Section]  GridActions:   TypeCount + (TypeFullName, MethodCount + (MethodName, Caption, IconTypeName)[])[]
  /// [Section]  ContextMenus:  TypeCount + (TypeFullName, MethodCount + (MethodName, Caption, IconTypeName)[])[]
  /// </summary>
  public static class MetadataFormat
  {
    public const int MagicNumber = 0x424C5A45; // "BLZE"
    public const ushort FormatVersion = 1;

    public sealed class MenuItemEntry
    {
      public string TypeFullName { get; set; } = "";
      public string Title { get; set; } = "";
      public string Route { get; set; } = "";
      public string IconTypeName { get; set; } = "";
      public string Group { get; set; } = "Default";
      public int OrderSequence { get; set; }
    }

    public sealed class FooterLinkEntry
    {
      public string TypeFullName { get; set; } = "";
      public string Title { get; set; } = "";
      public string Route { get; set; } = "";
      public string IconTypeName { get; set; } = "";
      public bool OpenNewWindow { get; set; } = true;
    }

    public sealed class ActionEntry
    {
      public string MethodName { get; set; } = "";
      public string Caption { get; set; } = "";
      public string Group { get; set; } = "Default";
      public string IconTypeName { get; set; } = "";
    }

    public sealed class TypeActions
    {
      public string TypeFullName { get; set; } = "";
      public List<ActionEntry> Actions { get; set; } = new List<ActionEntry>();
    }

    public sealed class AssemblyMetadataFile
    {
      public List<MenuItemEntry> MenuItems { get; set; } = new List<MenuItemEntry>();
      public List<FooterLinkEntry> FooterLinks { get; set; } = new List<FooterLinkEntry>();
      public List<TypeActions> PageActions { get; set; } = new List<TypeActions>();
      public List<TypeActions> GridActions { get; set; } = new List<TypeActions>();
      public List<TypeActions> ContextMenus { get; set; } = new List<TypeActions>();
    }

    public static void Write(Stream stream, AssemblyMetadataFile metadata)
    {
      using (var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true))
      {
        // Header
        writer.Write(MagicNumber);
        writer.Write(FormatVersion);

        // MenuItems
        writer.Write(metadata.MenuItems.Count);
        foreach (var item in metadata.MenuItems)
        {
          writer.Write(item.TypeFullName);
          writer.Write(item.Title);
          writer.Write(item.Route);
          writer.Write(item.IconTypeName);
          writer.Write(item.Group);
          writer.Write(item.OrderSequence);
        }

        // FooterLinks
        writer.Write(metadata.FooterLinks.Count);
        foreach (var item in metadata.FooterLinks)
        {
          writer.Write(item.TypeFullName);
          writer.Write(item.Title);
          writer.Write(item.Route);
          writer.Write(item.IconTypeName);
          writer.Write(item.OpenNewWindow);
        }

        // PageActions
        WriteTypeActions(writer, metadata.PageActions);

        // GridActions
        WriteTypeActions(writer, metadata.GridActions);

        // ContextMenus
        WriteTypeActions(writer, metadata.ContextMenus);
      }
    }

    public static AssemblyMetadataFile Read(Stream stream)
    {
      var metadata = new AssemblyMetadataFile();

      using (var reader = new BinaryReader(stream, Encoding.UTF8, leaveOpen: true))
      {
        // Header
        var magic = reader.ReadInt32();
        if (magic != MagicNumber)
          throw new InvalidDataException($"Invalid BlazorEngine metadata file (magic: 0x{magic:X8}, expected: 0x{MagicNumber:X8})");

        var version = reader.ReadUInt16();
        if (version > FormatVersion)
          throw new InvalidDataException($"Unsupported metadata format version {version} (max supported: {FormatVersion})");

        // MenuItems
        var menuCount = reader.ReadInt32();
        for (int i = 0; i < menuCount; i++)
        {
          metadata.MenuItems.Add(new MenuItemEntry
          {
            TypeFullName = reader.ReadString(),
            Title = reader.ReadString(),
            Route = reader.ReadString(),
            IconTypeName = reader.ReadString(),
            Group = reader.ReadString(),
            OrderSequence = reader.ReadInt32()
          });
        }

        // FooterLinks
        var footerCount = reader.ReadInt32();
        for (int i = 0; i < footerCount; i++)
        {
          metadata.FooterLinks.Add(new FooterLinkEntry
          {
            TypeFullName = reader.ReadString(),
            Title = reader.ReadString(),
            Route = reader.ReadString(),
            IconTypeName = reader.ReadString(),
            OpenNewWindow = reader.ReadBoolean()
          });
        }

        // PageActions
        metadata.PageActions = ReadTypeActions(reader);

        // GridActions
        metadata.GridActions = ReadTypeActions(reader);

        // ContextMenus
        metadata.ContextMenus = ReadTypeActions(reader);
      }

      return metadata;
    }

    private static void WriteTypeActions(BinaryWriter writer, List<TypeActions> typeActions)
    {
      writer.Write(typeActions.Count);
      foreach (var ta in typeActions)
      {
        writer.Write(ta.TypeFullName);
        writer.Write(ta.Actions.Count);
        foreach (var action in ta.Actions)
        {
          writer.Write(action.MethodName);
          writer.Write(action.Caption);
          writer.Write(action.Group);
          writer.Write(action.IconTypeName);
        }
      }
    }

    private static List<TypeActions> ReadTypeActions(BinaryReader reader)
    {
      var count = reader.ReadInt32();
      var result = new List<TypeActions>(count);
      for (int i = 0; i < count; i++)
      {
        var ta = new TypeActions
        {
          TypeFullName = reader.ReadString()
        };

        var actionCount = reader.ReadInt32();
        for (int j = 0; j < actionCount; j++)
        {
          ta.Actions.Add(new ActionEntry
          {
            MethodName = reader.ReadString(),
            Caption = reader.ReadString(),
            Group = reader.ReadString(),
            IconTypeName = reader.ReadString()
          });
        }

        result.Add(ta);
      }
      return result;
    }
  }
}
