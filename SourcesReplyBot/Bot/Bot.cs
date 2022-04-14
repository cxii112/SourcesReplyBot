using System.Collections.Generic;
using SourcesReplyBot.Models;

namespace SourcesReplyBot.Bot
{
    public class Bot
    {
        private static readonly BotAction _usage = new UsageAction();
        public static readonly DefaultAction Default = new();
        
        public static Dictionary<string, BotAction> Actions { get; } = new()
                                                                        {
                                                                            { "start", _usage},
                                                                            { "usage", _usage},
                                                                            { "book", new BookAction() },
                                                                            { "ping", new PingAction()}
                                                                        };
    }
}