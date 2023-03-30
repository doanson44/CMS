using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CMS.WebApi.AuthEndpoints;
using CMS.WebApi.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CMS.WebApi.SlackNotification;

public class SlackClient : ISlackClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly SlackNotificationSettings _slackNotificationSettings;

    public SlackClient(IHttpClientFactory httpClientFactory, IOptions<SlackNotificationSettings> slackNotificationSettings)
    {
        _httpClientFactory = httpClientFactory;
        _slackNotificationSettings = slackNotificationSettings.Value;
    }

    public async Task SendMessageAsync(SendSlackMessageRequest request)
    {
        using var _httpClient = _httpClientFactory.CreateClient();
        var json = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        using var httpResponse =
            await _httpClient.PostAsync(_slackNotificationSettings.WebHookUrl, json);

        httpResponse.EnsureSuccessStatusCode();
    }
}
