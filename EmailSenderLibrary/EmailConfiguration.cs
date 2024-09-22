using System.ComponentModel.DataAnnotations;

namespace EmailSenderLibrary;

/// <summary>
/// The EmailConfiguration class defines properties for configuring email settings including display name, sender email address, host, port, username, password, and SSL usage.
/// </summary>
public class EmailConfiguration
{
    /// <summary>
    ///The DisplayName property is the name of the email sender that will appear in the receiver's email client.
    /// </summary>
    public string DisplayName { get; set; } = "";

    /// <summary>
    /// The From property is the email address from which you will send the email.
    /// </summary>
    [EmailAddress]
    public required string From { get; set; }
    /// <summary>
    /// Host is the name of the server that handles the delivery of email messages.
    /// </summary>
    public required string Host { get; set; }
    /// <summary>
    /// Port is the logical address of the server tha handles the delivery of email messages.
    /// </summary>
    public required int Port { get; set; }
    /// <summary>
    /// Username is your email address or the unique name used by email sending server.
    /// </summary>
    public required string UserName { get; set; }
    /// <summary>
    /// The Password property is the password for the email account from which email would be sent.
    /// </summary>
    public required string Password { get; set; }
    /// <summary>
    /// UseSsl properties check whether email server using SSL or not while sending email.
    /// </summary>
    public bool UseSsl { get; set; }
}
