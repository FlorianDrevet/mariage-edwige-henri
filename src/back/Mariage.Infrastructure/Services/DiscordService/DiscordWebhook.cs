using System.Text;
using System.Text.Json;
using Mariage.Application.Common.Interfaces.Services;
using Microsoft.Extensions.Options;

namespace Mariage.Infrastructure.Services.DiscordService;

public class DiscordWebhook(IOptions<DiscordWebhookSettings> discordWebhookSettings): IDiscordWebhook
{
    public async Task SendDiscordWebhook(string message)
    {
        using var httpClient = new HttpClient();
        var payload = new
        {
            content = message
        };

        var json = JsonSerializer.Serialize(payload);
        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

        await httpClient.PostAsync(discordWebhookSettings.Value.WebhookUrl, httpContent);
    }
}