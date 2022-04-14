using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SourcesReplyBot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace SourcesReplyBot.Bot
{
    public class BotActions
    {
        public static async Task<Message> Start(ITelegramBotClient bot, Message message)
        {
            const string start = "Start:\n" +
                                 "Попробуй:" +
                                 "/usage";

            return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                  text: start,
                                                  replyMarkup: new ReplyKeyboardRemove());
        }

        public static async Task<Message> Usage(ITelegramBotClient bot, Message message)
        {
            const string usage = "Usage:\n" +
                                 "Здесь пока нихуя нет";

            return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                  text: usage,
                                                  replyMarkup: new ReplyKeyboardRemove());
        }

        public static async Task<Message> Get(ITelegramBotClient bot,
                                              Message            message,
                                              SourcesContext     context,
                                              ILogger            logger)
        {
            string key = "";
            string payload;
            try
            {
                key = message.Text.Split(" ")[1].ToLower();
                logger.LogInformation($"[BOT] /get {key}");
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

        public static async Task<Message> Default(ITelegramBotClient bot, Message message)
        {
            const string defaultMessage = "Нет такой команды. Попробуй:\n" +
                                          "/usage";

            return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                  text: defaultMessage,
                                                  replyMarkup: new ReplyKeyboardRemove());
        }
    }
}