using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SourcesReplyBot.Services;
using Telegram.Bot;

namespace SourcesReplyBot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            BotConfiguration = new BotConfiguration();
        }

        public IConfiguration Configuration { get; }

        public BotConfiguration BotConfiguration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
                                   {
                                       c.SwaggerDoc("v1",
                                                    new OpenApiInfo { Title = "SourcesReplyBot", Version = "v1" });
                                   });

            services.AddHostedService<ConfigureWebhook>();
            services.AddHttpClient("tgwebhook")
                    .AddTypedClient<ITelegramBotClient>(client =>
                                                            new TelegramBotClient(BotConfiguration.Token, client));

            services.AddScoped<HandleUpdatesService>();

            services.AddControllers()
                    .AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // app.UseSwagger();
                // app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SourcesReplyBot v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors();

            app.UseEndpoints(endpoints =>
                             {
                                 // Configure custom endpoint per Telegram API recommendations:
                                 // https://core.telegram.org/bots/api#setwebhook
                                 // If you'd like to make sure that the Webhook request comes from Telegram, we recommend
                                 // using a secret path in the URL, e.g. https://www.example.com/<token>.
                                 // Since nobody else knows your bot's token, you can be pretty sure it's us.
                                 endpoints.MapControllerRoute(name: "tgwebhook",
                                                              pattern: $"bot{BotConfiguration.Token}",
                                                              new { controller = "Webhook", action = "Post" });
                                 endpoints.MapControllers();
                             })
                ;
        }
    }
}