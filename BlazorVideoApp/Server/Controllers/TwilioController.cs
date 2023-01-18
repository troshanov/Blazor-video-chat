using Microsoft.AspNetCore.Mvc;
using BlazorVideoApp.Server.Services;

namespace BlazorVideoApp.Server.Controllers
{
    [
        ApiController,
        Route("api/twilio")
    ]
    public class TwilioController : ControllerBase
    {
        [HttpGet("token")]
        public IActionResult GetToken(
            [FromServices] TwilioService twilioService) =>
             new JsonResult(twilioService.GetTwilioJwt(User.Identity.Name));

        [HttpGet("rooms")]
        public async Task<IActionResult> GetRooms(
            [FromServices] TwilioService twilioService) =>
            new JsonResult(await twilioService.GetAllRoomsAsync());
    }
}
