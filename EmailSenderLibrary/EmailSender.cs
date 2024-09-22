using MailKit.Net.Smtp;
using MimeKit;

namespace EmailSenderLibrary;
/// <summary>
/// This class implements the IEmailSender interface to send email message
/// </summary>
/// <param name="emailConfiguration">the <see cref="EmailConfiguration"/> that represent the email configuration of sender such as username, display name, password</param>
public class EmailSender(EmailConfiguration emailConfiguration) : IEmailSender
{
    private readonly EmailConfiguration emailConfiguration = emailConfiguration;

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
        emailMessage.From.Add(new MailboxAddress(emailConfiguration.DisplayName, emailConfiguration.From));
        emailMessage.To.AddRange(emailInfo.SendTo.Select(ma => new MailboxAddress(ma.DisplayName, ma.Address)));
        emailMessage.Subject = emailInfo.MailSubject;
        var bodyBuilder = new BodyBuilder { HtmlBody = emailInfo.MailBody };

        if (emailInfo.Attachments != null && emailInfo.Attachments.Count > 0)
        {
            byte[] fileBytes;

            foreach (var attachment in emailInfo.Attachments)
            {
                using (var ms = new MemoryStream())
                {
                    attachment.CopyTo(ms);
                    fileBytes = ms.ToArray();
                }
                bodyBuilder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
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
            client.Connect(emailConfiguration.Host, emailConfiguration.Port, emailConfiguration.UseSsl);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            client.Authenticate(emailConfiguration.UserName, emailConfiguration.Password);

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
            await client.ConnectAsync(emailConfiguration.Host, emailConfiguration.Port, emailConfiguration.UseSsl);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            await client.AuthenticateAsync(emailConfiguration.UserName, emailConfiguration.Password);

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
