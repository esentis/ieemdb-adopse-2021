namespace Esentis.Ieemdb.Web.Helpers
{
  using System.Threading.Tasks;

  using Microsoft.AspNetCore.Identity.UI.Services;

  using SendGrid;
  using SendGrid.Helpers.Mail;

  public class SendGridEmailSender : IEmailSender
  {
    private readonly ISendGridClient client;

    public SendGridEmailSender(string apiKey)
      => client = new SendGridClient(apiKey);

    #region Implementation of IEmailSender

    /// <inheritdoc />
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
      var from = new EmailAddress("ieemdb@kritikos.io", "Ieemdb API");
      var to = new EmailAddress(email);

      var msg = MailHelper.CreateSingleEmail(
        from,
        to,
        subject,
        htmlContent: htmlMessage,
        plainTextContent: string.Empty);

      await client.SendEmailAsync(msg);
    }

    #endregion
  }
}
