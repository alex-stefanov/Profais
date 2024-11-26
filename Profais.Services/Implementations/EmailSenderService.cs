using Profais.Services.Interfaces;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Options;
using Profais.Common.Options;
using Profais.Data.Models;

namespace Profais.Services.Implementations;

public class EmailSenderService(
    IOptions<SmtpSettings> smtpSettings)
    : IEmailSenderService
{
    private readonly SmtpSettings _smtpSettings = smtpSettings.Value;

    public async Task SendEmailAsync(
        string subject,
        string body)
    {
        using var client = new SmtpClient(_smtpSettings.Server, _smtpSettings.Port);

        client.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
        client.EnableSsl = _smtpSettings.EnableSsl;

        var from = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName);
        var to = _smtpSettings.EmailsTo.Select(email => new MailAddress(email)).ToList();
        var message = new MailMessage
        {
            From = from,
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        foreach (var recipient in to)
        {
            message.To.Add(recipient);
        }

        await client.SendMailAsync(message);
    }
}