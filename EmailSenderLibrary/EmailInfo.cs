using Microsoft.AspNetCore.Http;
using System.Net.Mail;

namespace EmailSenderLibrary;

/// <summary>
/// This class is used to define the info of recipient's mailbox
/// </summary>
/// <param name="mailBoxes">the email addresses of recipients</param>
/// <param name="subject">the subject of email</param>
/// <param name="content">the mail body</param>
/// <param name="files">the attachment for the email</param>
public class EmailInfo(IEnumerable<string> mailBoxes, string subject, string content, IFormFileCollection files)
{
    /// <summary>
    /// The list of email recipients.
    /// </summary>
    public List<MailAddress> SendTo { get; set; } = [.. mailBoxes.Select(x => new MailAddress(x))];

    /// <summary>
    /// The subject of email
    /// </summary>
    public string MailSubject { get; set; } = subject;

    /// <summary>
    /// The body of email
    /// </summary>
    public string MailBody { get; set; } = content;

    /// <summary>
    /// The attachment of email
    /// </summary>
    public IFormFileCollection Attachments { get; set; } = files;
}
