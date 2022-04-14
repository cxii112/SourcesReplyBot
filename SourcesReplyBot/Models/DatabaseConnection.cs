using System;

namespace SourcesReplyBot.Models
{
    public class DatabaseConnection
    {
        public string Host { get; }
        public string User { get; }
        public string Name { get; }
        public string Password { get; }

        public DatabaseConnection()
        {
            Host = Environment.GetEnvironmentVariable("DATABASE_HOST");
            User = Environment.GetEnvironmentVariable("DATABASE_USER");
            Name = Environment.GetEnvironmentVariable("DATABASE_NAME");
            Password = Environment.GetEnvironmentVariable("DATABASE_PASSWORD");
        }
    }
}