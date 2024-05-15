using BlazorGenerator.Models;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGenerator.Utils
{
  internal class ExcelUtilities
  {

    internal static Stream? ExportToExcel<T>(List<T> data, List<VisibleField<T>> visibleFields) where T : class
    {
      XLWorkbook wb = new XLWorkbook();
      wb.Worksheets.Add(ToDataTable(data, visibleFields), "Export");
      Stream stream = new MemoryStream(); 
      wb.SaveAs(stream);
      stream.Position = 0;
      return stream;
    }

    static DataTable ToDataTable<T>(IList<T> data, List<VisibleField<T>> visibleFields)
    {
      PropertyDescriptorCollection properties =
          TypeDescriptor.GetProperties(data!.First()!.GetType());
      DataTable table = new DataTable();
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
