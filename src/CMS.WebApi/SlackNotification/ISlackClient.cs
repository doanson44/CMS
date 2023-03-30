using System.Threading.Tasks;
using CMS.WebApi.Models;

namespace CMS.WebApi.SlackNotification;

public interface ISlackClient
{
    Task SendMessageAsync(SendSlackMessageRequest request);
}
