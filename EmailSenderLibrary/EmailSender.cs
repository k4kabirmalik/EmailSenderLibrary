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

    private MimeMessage CreateEmailMessage(EmailInfo emailInfo)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(emailConfiguration.DisplayName, emailConfiguration.From));
        emailMessage.To.AddRange(emailInfo.SendTo.Select(ma => new MailboxAddress(ma.DisplayName, ma.Address)));
        emailMessage.Subject = emailInfo.MailSubject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = emailInfo.MailBody };
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
}
