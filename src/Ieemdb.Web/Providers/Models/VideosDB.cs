namespace Esentis.Ieemdb.Web.Providers.Models
{

  public class VideoResults
  {
    public int id { get; set; }
    public VideoDB[] results { get; set; }
  }

  public class VideoDB
  {
    public string id { get; set; }
    public string iso_639_1 { get; set; }
    public string iso_3166_1 { get; set; }
    public string key { get; set; }
    public string name { get; set; }
    public string site { get; set; }
    public int size { get; set; }
    public string type { get; set; }
  }

}
