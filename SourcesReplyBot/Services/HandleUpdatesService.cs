using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SourcesReplyBot.Bot;
using SourcesReplyBot.Models;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static SourcesReplyBot.Bot.Bot;

namespace SourcesReplyBot.Services
{
    public class HandleUpdatesService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<HandleUpdatesService> _logger;
        private readonly SourcesContext _context;

        public HandleUpdatesService(ITelegramBotClient            botClient,
                                    ILogger<HandleUpdatesService> logger)
        {
            _botClient = botClient;
            _logger = logger;
            _context = new SourcesContext();
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
            string key = message.Text!.Split(" ")[0].Replace("/", "");
            // string commandCommand = Actions[key].Command.Command;
            // var action = key switch
            // {
            //     "/start" => Usage(_botClient, message),
            //     "/usage" => Usage(_botClient, message),
            //     _ => {
            //     Actions[key].Action(_botClient, message, _context, _logger)
            // }
            // };
            try
            {
                var action = Actions[key].Action(_botClient, message, _context, _logger);
                Message sentMessage = await action;
                _logger.LogInformation($"{DateTime.Now} Message {sentMessage.MessageId} sent");
            }
            catch (KeyNotFoundException)
            {
                await Default.Action(_botClient, message);
            }
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
                ApiRequestException apiRequestException =>
                    $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            _logger.LogWarning("HandleError: {ErrorMessage}", ErrorMessage);
            return Task.CompletedTask;
        }
    }
}