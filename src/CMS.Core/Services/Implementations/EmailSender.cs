using System.Threading.Tasks;
using CMS.Core.Services.Interfaces;

namespace CMS.Core.Services.Implementations;

public class EmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string message)
    {
        // TODO: Wire this up to actual email sending logic via SendGrid, local SMTP, etc.
        return Task.CompletedTask;
    }
}
