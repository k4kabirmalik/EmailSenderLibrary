# Getting Started

This class library is used to send emails from asp.net core project

## Table of Contents

- Configure
- Usage
- Example
- License
- Contact

## Configure

Initialize email configuration by using EmailConfiguration class

```C#
var emailConfiguration = new EmailConfiguration
{
    DisplayName = "SenderName",
    From = "SenderEmail@example.com",
    Host = "smtp.example.com",
    Port = 465,
    Username = "SenderUsername",
    Password = "SenderPassword",
    UseSsl = true
};
```

## Usage

Use EmailInfo class to create email message. This class constructor accept four arguments first one is list of email address, second email subject, third email body in string format the last attachment if any

```C#
 var newMessage = new EmailInfo(
     ["username@domain.com"],
     "Test Message async",
     string.Format($"<h1 style=\"text-color:red\">This is test message by {_emailSender}</h1>"),
     null);
```

Use EmailSender class to send email message. This class implement IEmailSender interface.

```C#
private readonly IEmailSender _emailSender;

 // Send email
 _emailSender.SendEmail(newMessage);

 // Send email asynchronously
 await _emailSender.SendEmailAsync(newMessage);
```

To send attachment use FormFileCollection

```C#
var files = Request.Form.Files.Any() ? Request.Form.Files : new FormFileCollection();

 var newMessage = new EmailInfo(
     ["username@domain.com"],
     "Test Message async",
     string.Format($"<h1 style=\"text-color:red\">This is test message by {_emailSender}</h1>"),
     files);

// Send email
 _emailSender.SendEmail(newMessage);

 // Send email asynchronously
 await _emailSender.SendEmailAsync(newMessage);
```

## Example

- Crate an Asp Web API Project.
- add this EmailSenderLibrary to project and reference it in main project
- add the following setting in appsettings.json file

```JSON
"EmailConfiguration": {
  "DisplayName": "Kabir Malik",
  "From": "kabirunofficial@gmail.com",
  "Host": "smtp.gmail.com",
  "Port": 465,
  "Username": "kabirunofficial@gmail.com",
  "password": "customPassword",
  "UseSsl": true
}
```

In program.cs file register services

```C#
var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
builder.Services.AddSingleton<EmailConfiguration>(emailConfig);
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.Configure<FormOptions>(options =>
{
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartBodyLengthLimit = int.MaxValue;
    options.MemoryBufferThreshold = int.MaxValue;
});

```

- In the class that will use to send email define IEmailSender

```C#
private readonly IEmailSender _emailSender;

public WeatherForecastController(IEmailSender emailSender)
{
    _emailSender = emailSender; ;
}
```

- call email sender functions to send email

```C#
public async Task Post()
{
    var files = Request.Form.Files.Any() ? Request.Form.Files : new FormFileCollection();

    var newMessage = new EmailInfo(
                    ["username@domain.com"],
                    "Test Message async",
                    string.Format($"<h1 style=\"text-color:red\">This is test message by {_emailSender}</h1>"),
                    files);

    // Send email
    _emailSender.SendEmail(newMessage);

    // Send email asynchronously
    await _emailSender.SendEmailAsync(newMessage);
}
````

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE.md) file for details.

## Contact

Email: <k4kabirmalik@outlook.com>\
Github: [Kabir Malik](https://github.com/k4kabirmalik)
