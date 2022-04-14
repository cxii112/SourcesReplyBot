using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SourcesReplyBot.Models
{
    public abstract class BotAction
    {
        public virtual string? ExtraDescription { get; }
        public virtual BotCommand Command { get; init; }

        public abstract Task<Message> Action(ITelegramBotClient bot,
                                            Message            message,
                                            SourcesContext?    context,
                                            ILogger?           logger);
    }
}