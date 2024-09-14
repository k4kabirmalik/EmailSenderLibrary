using MailKit.Net.Smtp;
using MimeKit;

namespace EmailSenderLibrary;

public class EmailSender(EmailConfiguration emailConfiguration) : IEmailSender
{
    private readonly EmailConfiguration emailConfiguration = emailConfiguration;

    public void SendEmail(EmailInfo emailInfo)
    {
        var emailMessage = CreateEmailMessage(emailInfo);
        Send(emailMessage);
    }

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
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = emailInfo.MailBody };
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
