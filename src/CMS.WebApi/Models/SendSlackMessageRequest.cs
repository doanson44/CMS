using Newtonsoft.Json;

namespace CMS.WebApi.Models;

public class SendSlackMessageRequest
{
    [JsonProperty("channel")]
    public string Channel;

    [JsonProperty("username")]
    public string Username;

    [JsonProperty("text")]
    public string Text;

    [JsonProperty("icon_emoji")]
    public string IconEmoji;
}
