using System.Threading.Tasks;

namespace CMS.Core.Services.Interfaces;

public interface IEmailSender
{
    Task SendEmailAsync(string email, string subject, string message);
}
