using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SourcesReplyBot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace SourcesReplyBot.Bot
{
    public class PingAction : BotAction
    {
        public PingAction()
        {
            Command = new BotCommand
                      {
                          Command = "ping", Description = "Ping test"
                      };
        }

        public override BotCommand Command { get; init; }

        public override async Task<Message> Action(ITelegramBotClient bot, 
                                                   Message            message, 
                                                   SourcesContext? context, 
                                                   ILogger? logger)
        {
            const string pong = "pong";

            return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                  text: pong,
                                                  replyMarkup: new ReplyKeyboardRemove());
        }
    }
}