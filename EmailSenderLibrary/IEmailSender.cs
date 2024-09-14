namespace EmailSenderLibrary;

public interface IEmailSender
{
    void SendEmail(EmailInfo emailInfo);
    Task SendEmailAsync(EmailInfo emailInfo);
}
