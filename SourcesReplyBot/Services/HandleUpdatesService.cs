using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static SourcesReplyBot.Bot.BotActions;

namespace SourcesReplyBot.Services
{
    public class HandleUpdatesService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<HandleUpdatesService> _logger;

        public HandleUpdatesService(ITelegramBotClient botClient, ILogger<HandleUpdatesService> logger)
        {
            _botClient = botClient;
            _logger = logger;
        }

        public async Task EchoAsync(Update update)
        {
            var handler = update.Type switch
            {
                UpdateType.Message => BotOnMessageUpdate(update.Message),
                _ => UnknownUpdateHandlerAsync(update)
            };
            try
            {
                await handler;
            }
            catch (Exception e)
            {
                HandleErrorAsync(e);
            }
        }

        private async Task BotOnMessageUpdate(Message message)
        {
            _logger.LogInformation($"{DateTime.Now} [MESSAGE] {message.Type} by {message.From.Username}");
            if (message.Type != MessageType.Text) return;
            var action  = message.Text!.Split(" ")[0] switch
            {
                "/start" => Start(_botClient, message),
                "/usage" => Usage(_botClient,message),
                _ => Default(_botClient, message)
            };
            Message sentMessage = await action;
            _logger.LogInformation($"{DateTime.Now} Message {sentMessage.MessageId} sent");
        }


        private Task UnknownUpdateHandlerAsync(Update update)
        {
            _logger.LogInformation("Unknown update type: {updateType}", update.Type);
            return Task.CompletedTask;
        }
        public Task HandleErrorAsync(Exception exception)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            _logger.LogWarning("HandleError: {ErrorMessage}", ErrorMessage);
            return Task.CompletedTask;
        }
    }
}