namespace EmailSenderLibrary;

/// <summary>
/// This interface defines the methods for sending emails
/// </summary>
public interface IEmailSender
{
    /// <summary>
    /// This function sends an email using the provided <see cref="EmailInfo"/> object.
    /// </summary>
    /// <param name="emailInfo">The <see cref="EmailInfo"/> parameter likely contains information needed to send an
    /// email, such as the recipient's email address, subject, body, attachments, etc.</param>
    void SendEmail(EmailInfo emailInfo);
    /// <summary>
    /// The SendEmailAsync function sends an email using the provided <see cref="EmailInfo"/>.
    /// </summary>
    /// <param name="emailInfo">The <see cref="EmailInfo"/> parameter likely contains information needed to send an
    /// email, such as the recipient's email address, subject, body, attachments, etc.</param>
    Task SendEmailAsync(EmailInfo emailInfo);
}
