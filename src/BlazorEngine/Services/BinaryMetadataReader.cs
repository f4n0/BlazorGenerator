using System.Text;

namespace BlazorEngine.Services
{
  /// <summary>
  /// Reads the binary metadata format produced by BlazorEngine.BuildTasks at compile time.
  /// This is the runtime counterpart of MetadataFormat — lightweight, no external dependencies.
  /// </summary>
  internal static class BinaryMetadataReader
  {
    private const int MagicNumber = 0x424C5A45; // "BLZE"
    private const ushort MaxSupportedVersion = 1;

    internal sealed class MenuItemEntry
    {
      public string TypeFullName { get; set; } = "";
      public string Title { get; set; } = "";
      public string Route { get; set; } = "";
      public string IconTypeName { get; set; } = "";
      public string Group { get; set; } = "Default";
      public int OrderSequence { get; set; }
    }

    internal sealed class FooterLinkEntry
    {
      public string TypeFullName { get; set; } = "";
      public string Title { get; set; } = "";
      public string Route { get; set; } = "";
      public string IconTypeName { get; set; } = "";
      public bool OpenNewWindow { get; set; } = true;
    }

    internal sealed class ActionEntry
    {
      public string MethodName { get; set; } = "";
      public string Caption { get; set; } = "";
      public string Group { get; set; } = "Default";
      public string IconTypeName { get; set; } = "";
    }

    internal sealed class TypeActions
    {
      public string TypeFullName { get; set; } = "";
      public List<ActionEntry> Actions { get; set; } = new();
    }

    internal sealed class AssemblyMetadata
    {
      public List<MenuItemEntry> MenuItems { get; set; } = new();
      public List<FooterLinkEntry> FooterLinks { get; set; } = new();
      public List<TypeActions> PageActions { get; set; } = new();
      public List<TypeActions> GridActions { get; set; } = new();
      public List<TypeActions> ContextMenus { get; set; } = new();
    }

    internal static AssemblyMetadata Read(Stream stream)
    {
      var metadata = new AssemblyMetadata();

      using var reader = new BinaryReader(stream, Encoding.UTF8, leaveOpen: true);

      // Header
      var magic = reader.ReadInt32();
      if (magic != MagicNumber)
        throw new InvalidDataException(
          $"Invalid BlazorEngine metadata (magic: 0x{magic:X8}, expected: 0x{MagicNumber:X8})");

      var version = reader.ReadUInt16();
      if (version > MaxSupportedVersion)
        throw new InvalidDataException(
          $"Unsupported metadata version {version} (max: {MaxSupportedVersion})");

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

      return metadata;
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
