using MailKit.Net.Smtp;
using MimeKit;

namespace EmailSenderLibrary;
/// <summary>
/// This class implements the IEmailSender interface to send email message
/// </summary>
/// <param name="smtpOptions">the <see cref="SmtpOptions"/> that represent the email configuration of sender such as username, display name, password</param>
public class EmailSender(SmtpOptions smtpOptions) : IEmailSender
{
    /// <summary>
    /// The SendEmail function creates an email message using the provided <see cref="EmailInfo"/> and sends it.
    /// </summary>
    /// <param name="emailInfo">The <see cref="EmailInfo"/> parameter likely contains information needed to send an
    /// email, such as the recipient's email address, subject, body, attachments, etc.</param>
    public void SendEmail(EmailInfo emailInfo)
    {
        var emailMessage = CreateEmailMessage(emailInfo);
        Send(emailMessage);
    }

    /// <summary>
    /// The SendEmailAsync function create an email message using the provided <see cref="EmailInfo"/>  sends an email asynchronously.
    /// </summary>
    /// <param name="emailInfo">The <see cref="EmailInfo"/> parameter likely contains information needed to send an
    /// email, such as the recipient's email address, subject, body, attachments, etc.</param>
    public async Task SendEmailAsync(EmailInfo emailInfo)
    {
        var emailMessage = CreateEmailMessage(emailInfo);
        await SendAsync(emailMessage);
    }

    private MimeMessage CreateEmailMessage(EmailInfo emailInfo)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(smtpOptions.DisplayName, smtpOptions.From));
        emailMessage.To.AddRange(emailInfo.SendTo.Select(ma => new MailboxAddress(ma.DisplayName, ma.Address)));
        emailMessage.Subject = emailInfo.MailSubject;

        var bodyBuilder = new BodyBuilder { HtmlBody = emailInfo.MailBody };

        if (emailInfo.Attachments != null && emailInfo.Attachments.Any())
        {
            foreach (var attachment in emailInfo.Attachments)
            {
                if (attachment.ContentStream == Stream.Null) continue;

                if (attachment.ContentStream.CanSeek)
                {
                    attachment.ContentStream.Position = 0;
                }

                bodyBuilder.Attachments.Add(
                    attachment.FileName,
                    attachment.ContentStream,
                    ContentType.Parse(attachment.ContentType)
                );
            }
        }

        emailMessage.Body = bodyBuilder.ToMessageBody();
        return emailMessage;
    }
    private void Send(MimeMessage emailMessage)
    {
        using var client = new SmtpClient();
        try
        {
            client.Connect(smtpOptions.Host, smtpOptions.Port, smtpOptions.UseSsl);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            client.Authenticate(smtpOptions.UserName, smtpOptions.Password);

            client.Send(emailMessage);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
        finally
        {
            client.Disconnect(true);
            client.Dispose();
        }
    }

    private async Task SendAsync(MimeMessage emailMessage)
    {
        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(smtpOptions.Host, smtpOptions.Port, smtpOptions.UseSsl);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            await client.AuthenticateAsync(smtpOptions.UserName, smtpOptions.Password);

            await client.SendAsync(emailMessage);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
        finally
        {
            await client.DisconnectAsync(true);
            client.Dispose();
        }
    }
}
