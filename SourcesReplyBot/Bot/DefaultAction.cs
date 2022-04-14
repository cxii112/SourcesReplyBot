using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace SourcesReplyBot.Bot
{
    public class DefaultAction
    {
        public async Task<Message> Action(ITelegramBotClient bot, Message message)
        {
            const string defaultMessage = "Нет такой команды. Попробуй:\n" +
                                          "/usage";

            return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                  text: defaultMessage,
                                                  replyMarkup: new ReplyKeyboardRemove());
        }
    }
}