using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using static SourcesReplyBot.Bot.Bot;

namespace SourcesReplyBot.Services
{
    public class ConfigureWebhook : IHostedService
    {
        private readonly ILogger<ConfigureWebhook> _logger;
        private readonly IServiceProvider _services;
        private readonly BotConfiguration _botConfiguration;

        public ConfigureWebhook(ILogger<ConfigureWebhook> logger,
                                IServiceProvider          services)
        {
            _logger = logger;
            _services = services;
            _botConfiguration = new BotConfiguration();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var updateTypes = new List<UpdateType>
                              {
                                  UpdateType.Message,
                                  UpdateType.Unknown
                              };
            using var scope = _services.CreateScope();
            var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();
            var webhookAddress = @$"{_botConfiguration.HostAddress}/bot{_botConfiguration.Token}";
            _logger.LogInformation($"Starting webhook: {webhookAddress}");
            SetCommands(botClient);
            await botClient.SetWebhookAsync(url: webhookAddress,
                                            allowedUpdates: Array.Empty<UpdateType>(),
                                            cancellationToken: cancellationToken);
        }

        private async void SetCommands(ITelegramBotClient botClient)
        {
            var commands = await botClient.GetMyCommandsAsync();
            var newCommands = Actions.Select(pair => pair.Value.Command);
            var union = commands.Union(newCommands);
            await botClient.SetMyCommandsAsync(union);
            _logger.LogInformation($"[BOT] Set {union.Count()} commands");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            using var scope = _services.CreateScope();
            var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();
            _logger.LogInformation($"Removing webhook");
            await botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
        }
    }
}