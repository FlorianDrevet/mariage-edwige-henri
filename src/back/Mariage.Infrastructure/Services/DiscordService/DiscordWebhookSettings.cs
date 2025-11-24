namespace Mariage.Infrastructure.Services.DiscordService;

public class DiscordWebhookSettings
{
    public const string SectionName = "DiscordWebhookSettings";
    public string WebhookUrl { get; init; } = null!;
}