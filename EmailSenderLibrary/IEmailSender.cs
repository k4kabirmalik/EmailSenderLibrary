namespace EmailSenderLibrary;

public interface IEmailSender
{
    // public bool SendEmail(EmailInfo emailInfo);

    void SendEmail(EmailInfo emailInfo);
}
