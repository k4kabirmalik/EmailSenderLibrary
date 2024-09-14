using System.ComponentModel.DataAnnotations;

namespace EmailSenderLibrary;

public class EmailConfiguration
{
    public string DisplayName { get; set; } = "";
    [EmailAddress]
    public required string From { get; set; }
    public required string Host { get; set; }
    public required int Port { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public bool UseSsl { get; set; }
}
