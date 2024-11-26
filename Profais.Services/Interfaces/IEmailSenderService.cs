namespace Profais.Services.Interfaces;

public interface IEmailSenderService
{
    Task SendEmailAsync(string subject, string body);
}
