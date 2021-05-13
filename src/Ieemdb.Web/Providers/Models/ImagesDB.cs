namespace Esentis.Ieemdb.Web.Providers.Models
{
  public class ImageResults
  {
    public int id { get; set; }
    public Poster[] posters { get; set; }
  }

  public class Poster
  {
    public string file_path { get; set; }
  }
}
