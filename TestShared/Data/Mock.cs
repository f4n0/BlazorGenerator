using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.FluentUI.AspNetCore.Components;

namespace TestShared.Data
{
  public class Mock : ICloneable
  {
    [Display(Name = "Id")]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    [Display(Name = "Order Date")]
    public DateTime? OrderDate { get; set; }
    public float Price { get; set; }
    public string NullTest { get; set; } = null!;
    public Type Icon { get; set; } = typeof(Icons.Regular.Size16.AddSquare);
    public MockEnum Type { get; set; }
    public bool Enabled { get; set; }

    public static Mock GetSingleMock()
    {
      var rand = new Random();
      const string strStrings = "Praesentium repellat fuga fuga possimus consequatur. Quia officia numquam ab facere. Dolorem quae eum dolorum sunt necessitatibus. Illo qui est enim eos quaerat sequi repudiandae laborum. Iure autem voluptate enim.";
      var words = strStrings.Split(' ');

      return new Mock()
      {
        Id = rand.Next(),
        Name = words[rand.Next(0, words.Length - 1)],
        OrderDate = DateTime.Now,
        Description = words[rand.Next(0, words.Length - 1)],
        Price = (float)rand.NextDouble(),
        Enabled = rand.NextDouble() >= 0.5
      };
    }

    public static List<Mock> GetMultipleMock(int itemCount = 10)
    {
      var data = new List<Mock>();
      for (int i = 0; i < itemCount; i++)
      {
        data.Add(GetSingleMock());
      }
      return data;
    }

    public object Clone()
    {
      return new Mock() { Id = Id, Name = Name, OrderDate = OrderDate, Price = Price };
    }
  }
}
