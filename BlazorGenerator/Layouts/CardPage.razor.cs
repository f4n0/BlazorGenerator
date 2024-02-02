using BlazorGenerator.Components.Base;
using BlazorGenerator.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGenerator.Layouts
{
  public partial class CardPage<T> : BlazorgenComponentBase, IDialogContentComponent<T>
  {
    public virtual int GridSize => 6;

    [Parameter]
    public T Content
    {
      get
      {
        return _data!;
      }
      set
      {
        _data = value;
        OriginalContent = value;
      }
    }
    private T? OriginalContent { get; set; }
    private T? _data;

    public List<VisibleField<T>> VisibleFields { get; set; } = [];

    void HandleSave(T content)
    {
      OnInsert(content);
      OnModify(content, OriginalContent!);
    }

    void HandleDiscard(T content)
    {
      OnDelete(content);
    }

    public virtual void OnInsert(T entity)
    {
    }

    public virtual void OnModify(T entity, T oldEntity)
    {
    }

    public virtual void OnDelete(T entity)
    {
    }
  }
}
