namespace Mariage.Application.Common.Interfaces.Services;

public interface IDiscordWebhook
{
    public Task SendDiscordWebhook(string message);
}