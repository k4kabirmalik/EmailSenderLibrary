using Microsoft.AspNetCore.Http;
using System.Net.Mail;

namespace EmailSenderLibrary;

public class EmailInfo(IEnumerable<string> to, string subject, string content, IFormFileCollection files)
{
    public List<MailAddress> SendTo { get; set; } = [.. to.Select(x => new MailAddress(x))];

    public string MailSubject { get; set; } = subject;

    public string MailBody { get; set; } = content;

    public IFormFileCollection Attachments { get; set; } = files;
}
