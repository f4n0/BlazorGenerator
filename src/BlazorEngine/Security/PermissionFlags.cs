namespace BlazorEngine.Security;

[Flags]
public enum PermissionFlags
{
  None = 0,
  Read = 1 << 0,
  Modify = 1 << 1,
  Delete = 1 << 2,
  Insert = 1 << 3,
  Execute = 1 << 4,
  All = Read | Modify | Delete | Insert | Execute
}
