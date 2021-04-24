namespace Esentis.Ieemdb.Web.Views.Shared
{
  public class EmailButtonViewModel
  {
    public EmailButtonViewModel(string text, string uri)
    {
      Text = text;
      Uri = uri;
    }

    public string Text { get; set; } = string.Empty;

    public string Uri { get; set; } = string.Empty;
  }
}
