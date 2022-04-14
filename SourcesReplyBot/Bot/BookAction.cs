using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SourcesReplyBot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace SourcesReplyBot.Bot
{
    public class BookAction : BotAction
    {
        public BookAction()
        {
            Command = new BotCommand
                      {
                          Command = "book",
                          Description = "Находит книгу/учебник по ключу"
                      };
            ExtraDescription = "Для использования напиши\n" +
                               $"`/{Command.Command} <ключ>`\n" +
                               $"Если все хорошо, ты получишь ссылку на файл";
        }

        public override string? ExtraDescription { get; } 
        public override BotCommand Command { get; init; }

        public override async Task<Message> Action(ITelegramBotClient bot,
                                                   Message            message,
                                                   SourcesContext     context,
                                                   ILogger            logger)
        {
            string key = "";
            string payload;
            try
            {
                key = message.Text.Split(" ")[1].ToLower();
                logger.LogInformation($"[BOT] /{Command.Command} {key}");
                Source source = context.sources.Find(key);
                // Source source = context.sources.Where(source => source.key == key).First();
                payload = "Держи\n" +
                          $"{source.key}: {source.description}\n" +
                          $"{source.link}";
            }
            catch (IndexOutOfRangeException)
            {
                logger.LogInformation($"[BOT] Missing <key> in message");
                payload = "Не указан ключ.";
            }
            catch (NullReferenceException)
            {
                logger.LogInformation($"[BOT] {key} not found");
                payload = $"По запросу \"{key}\" ничего не найдено.";
            }

            string getMessage = payload;
            return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                  text: getMessage,
                                                  replyToMessageId: message.MessageId,
                                                  replyMarkup: new ReplyKeyboardRemove());
        }
    }
}