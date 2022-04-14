using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SourcesReplyBot.Services;
using Telegram.Bot.Types;

namespace SourcesReplyBot.Controllers
{
    public class WebhookController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromServices] HandleUpdatesService handleUpdatesService,
                                              [FromBody]     Update              update)
        {
            await handleUpdatesService.EchoAsync(update);
            return Ok();
        }
    }
}