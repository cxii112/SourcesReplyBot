using Microsoft.EntityFrameworkCore;

namespace SourcesReplyBot.Models
{
    public class SourcesContext : DbContext
    {
        private readonly DatabaseConnection _connection;

        public SourcesContext()
        {
            _connection = new DatabaseConnection();
        }

        public DbSet<Source> sources { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder
                .UseNpgsql($@"Host={_connection.Host};Database={_connection.Name};Username={_connection.User};Password={_connection.Password}");
        }
    }
}