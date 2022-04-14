using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SourcesReplyBot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace SourcesReplyBot.Bot
{
    public class UsageAction : BotAction
    {
        public UsageAction()
        {
            Command = new BotCommand
                      {
                          Command = "usage",
                          Description = "Использование бота"
                      };
        }

        public override BotCommand Command { get; init; }

        public override async Task<Message> Action(ITelegramBotClient bot,
                                                   Message            message,
                                                   SourcesContext?    context,
                                                   ILogger?           logger)
        {
            string usage = "*Использование:*\n";
            var descriptions = Bot.Actions.Select(pair => pair.Value)
                                  .Where(action => action.Command.Command is not ("usage" or "start"))
                                  .Select(action => GenetareVerboseDescription(action));
            usage += string.Join("\n", descriptions);
            return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                  text: usage,
                                                  replyMarkup: new ReplyKeyboardRemove(),
                                                  parseMode: ParseMode.MarkdownV2);
        }

        private static string GenetareVerboseDescription(BotAction action)
        {
            string command = action.Command.Command;
            string description = action.Command.Description;
            string? extraDescription = action.ExtraDescription;
            return $"/{command} " +
                   $"_{description}_\n" + // "__" for italic font
                   $"{extraDescription}\n";
        }
    }
}