using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace SourcesReplyBot.Bot
{
    public class BotActions
    {
        public static async Task<Message> Usage(ITelegramBotClient bot, Message message)
        {
            const string usage = "Usage:\n" +
                                 "Здесь пока нихуя нет";

            return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                  text: usage,
                                                  replyMarkup: new ReplyKeyboardRemove());
        }
        public static async Task<Message> Default(ITelegramBotClient bot, Message message)
        {
            const string usage = "Нет такой команды. Попробуй:\n" +
                                 "/usage";

            return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                  text: usage,
                                                  replyMarkup: new ReplyKeyboardRemove());
        }
        public static async Task<Message> Start(ITelegramBotClient bot, Message message)
        {
            const string usage = "Start:\n" +
                                 "Попробуй:" +
                                 "/usage";

            return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                  text: usage,
                                                  replyMarkup: new ReplyKeyboardRemove());
        }
    }
}