using System.Net.Mail;
using MimeKit;

namespace EmailSenderLibrary;

public class EmailInfo(IEnumerable<string> to, string subject, string content)
{
    public List<MailAddress> SendTo { get; set; } = [.. to.Select(x => new MailAddress(x))];

    public string MailSubject { get; set; } = subject;

    public string MailBody { get; set; } = content;
}
