namespace TestShared.Data
{
  public class Mock
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime? OrderDate { get; set; }
    public float Price { get; set; }
    public string NullTest { get; set; } = null!;

    public MockEnum type { get; set; }

    public static Mock getSingleMock()
    {
      var rand = new Random();
      string strStrings = "Praesentium repellat fuga fuga possimus consequatur. Quia officia numquam ab facere. Dolorem quae eum dolorum sunt necessitatibus. Illo qui est enim eos quaerat sequi repudiandae laborum. Iure autem voluptate enim.";
      var words = strStrings.Split(' ');

      return new Mock()
      {
        Id = rand.Next(),
        Name = words[rand.Next(0, words.Length - 1)],
        OrderDate = DateTime.Now,
        Description = words[rand.Next(0, words.Length - 1)],
        Price = (float)rand.NextDouble()
      };
    }

    public static List<Mock> getMultipleMock()
    {
      return new List<Mock>()
            {
                getSingleMock(),
                getSingleMock(),
                getSingleMock(),
                getSingleMock(),
                getSingleMock(),
                getSingleMock(),
                getSingleMock(),
                getSingleMock(),
                getSingleMock(),
                getSingleMock(),
            };
    }
  }
}
