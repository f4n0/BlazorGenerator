using BlazorGenerator.Models;
using ClosedXML.Excel;
using System.ComponentModel;
using System.Data;

namespace BlazorGenerator.Utils
{
  internal static class ExcelUtilities
  {
    internal static Stream ExportToExcel<T>(List<T> data, List<VisibleField<T>> visibleFields) where T : class
    {
      XLWorkbook wb = new();
      wb.Worksheets.Add(ToDataTable(data, visibleFields), "Export");
      Stream stream = new MemoryStream();
      wb.SaveAs(stream);
      stream.Position = 0;
      return stream;
    }

    static DataTable ToDataTable<T>(IList<T> data, List<VisibleField<T>> visibleFields)
    {
      DataTable table = new();
      foreach (var field in visibleFields)
        table.Columns.Add(field.Caption, field.FieldType);

      foreach (T item in data)
      {
        DataRow row = table.NewRow();
        foreach (var field in visibleFields)
          row[field.Name] = field.Getter(item) ?? DBNull.Value;
        table.Rows.Add(row);
      }
      return table;
    }
  }
}
