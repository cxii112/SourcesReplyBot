using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

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
            await botClient.SetWebhookAsync(
                                            url: webhookAddress,
                                            allowedUpdates:Array.Empty<UpdateType>(),
                                            cancellationToken: cancellationToken);
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