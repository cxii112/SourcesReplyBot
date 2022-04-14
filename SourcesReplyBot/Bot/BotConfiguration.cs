using System;

namespace SourcesReplyBot
{
    public class BotConfiguration
    {
        public BotConfiguration()
        {
            Token = Environment.GetEnvironmentVariable("TOKEN");
            HostAddress = Environment.GetEnvironmentVariable("HOST_ADDRESS");
        }

        public string Token { get; }

        public string HostAddress { get; }
    }
}