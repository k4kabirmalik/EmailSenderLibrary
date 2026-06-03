# EmailSenderLibrary

A high-performance, transport-agnostic .NET 10 class library for sending emails using MailKit.

## Table of Contents

- [Requirements](#requirements)

- [Configuration](#configure)

- [Usage](#usage)

- [Working with Attachments](#working-with-attachments)

- [Example Integration](#example-integration)

- [License](#license)

## Requirements

- Framework: .NET 10+

- Engine: MailKit & MimeKit

## Configure

Initialize your SMTP settings using the `SmtpOptions` class.

```C#
var smtpOptions = new SmtpOptions
{
    DisplayName = "Sender Name",
    From = "sender@example.com",
    Host = "smtp.example.com",
    Port = 465,
    UserName = "YourUsername",
    Password = "YourPassword",
    UseSsl = true
};
```

## Usage

### 1. Create the Message

Use the `EmailInfo` class to define your recipients, subject, and content.

```C#
var recipients = new List<string> { "user@domain.com" };
var subject = "Tournament Update";
var body = "<h1>Match starts at 5 PM</h1>";

var email = new EmailInfo(recipients, subject, body, null);
```

### 2. Send the Email

The `EmailSender` class handles both synchronous and asynchronous operations.

```C#
// Synchronous
_emailSender.SendEmail(email);

// Asynchronous (Recommended)
await _emailSender.SendEmailAsync(email);
```

## Working with Attachments

To keep the core logic clean and decoupled from ASP.NET Core, this library uses a custom `EmailAttachment` class.

### Manual Attachment Creation

```C#
var attachment = new EmailAttachment
{
    FileName = "report.pdf",
    ContentType = "application/pdf",
    ContentStream = System.IO.File.OpenRead("path/to/file.pdf")
};

var email = new EmailInfo(recipients, subject, body, [attachment]);
```

### Converting from IFormFile (Web API)

If you are using this in an API project, map your uploaded files to the `EmailAttachment` model:

```c#
var attachments = Request.Form.Files.Select(f => new EmailAttachment
{
    FileName = f.FileName,
    ContentType = f.ContentType,
    ContentStream = f.OpenReadStream()
});
```

## Example Integration

### 1. AppSettings Configuration

```json
"SmtpOptions": {
  "DisplayName": "Admin",
  "From": "admin@tournament.com",
  "Host": "smtp.gmail.com",
  "Port": 465,
  "UserName": "admin@tournament.com",
  "Password": "your-app-password",
  "UseSsl": true
}
```

### 2. Dependency Injection (`Program.cs`)

```c#
var smtpConfig = builder.Configuration.GetSection("SmtpOptions").Get<SmtpOptions>();

builder.Services.AddSingleton(smtpConfig);
builder.Services.AddScoped<IEmailSender, EmailSender>();
```

### 3. Usage in Controller

```c#
[HttpPost]
public async Task<IActionResult> SendNotification()
{
    // Map web files to Core-friendly EmailAttachments
    var attachments = Request.Form.Files.Select(f => new EmailAttachment
    {
        FileName = f.FileName,
        ContentType = f.ContentType,
        ContentStream = f.OpenReadStream()
    }).ToList();

    var email = new EmailInfo(
        ["player@example.com"],
        "Welcome!",
        "<p>Glad to have you.</p>",
        attachments
    );

    await _emailSender.SendEmailAsync(email);
    return Ok();
}
```

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE.md) file for details.

## Contact

- Email: [Send Email](mailto:31293295+k4kabirmalik@users.noreply.github.com)
- Github: [Kabir Malik](https://github.com/k4kabirmalik)
