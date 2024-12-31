using BlazorGenerator.Models;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Reflection;
using System.Text;

namespace BlazorGenerator.Utils
{
  internal static class RoslynUtilities
  {
    internal static ScriptRunner<object> CreateAndInstatiateClass<T>(List<VisibleField<T>> visibleFields, string? title = null, CancellationToken ct = default)
    {
      string typeName = typeof(T).Name;
      var sb = new StringBuilder();
      foreach (var field in visibleFields)
      {
        var text = $"VisibleFields.AddField(\"{field.Name}\");";
        sb.Append(text);
      }
      const string classdeclaration = @"
        using BlazorGenerator.Layouts;
        using BlazorGenerator.Models;
        using BlazorGenerator.Utils;
        using System.Collections.Generic;
        using {3};
        public class TempListEditDialog : CardPage<{0}>
        {{

          public override string Title => ""{1}"";
          public override int GridSize => 12;

          protected override void OnParametersSet()
          {{
            VisibleFields = new List<VisibleField<{0}>>();
            {2}

            ShowActions = false;
            ShowButtons = false;
          }}
        }}
        return typeof(TempListEditDialog);
        ";
      var script = CSharpScript.Create(string.Format(classdeclaration, typeName, title, sb, typeof(T).Namespace), ScriptOptions.Default.WithReferences(Assembly.GetExecutingAssembly(), typeof(T).Assembly));
      script.Compile(ct);

      return script.CreateDelegate(ct);
      // run and you get Type object for your fresh type
      //return (Type)script.RunAsync().Result.ReturnValue;
    }
  }
}
