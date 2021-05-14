namespace Esentis.Ieemdb.Web.Helpers
{
  using System.Threading.Tasks;

  using Microsoft.AspNetCore.Identity.UI.Services;
  using Microsoft.Extensions.Logging;

  public class DummyEmailSender : IEmailSender
  {
    private const string SentEmail =
      "Sent email with subject {Subject} to address {Address} with content {HtmlMessage}";

    private readonly ILogger<IEmailSender> logger;

    public DummyEmailSender(ILogger<IEmailSender> logger)
      => this.logger = logger;

    #region Implementation of IEmailSender

    /// <inheritdoc />
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
      => logger.LogError(SentEmail, subject, email, htmlMessage);

    #endregion
  }
}
