using Chatapp.API.Controllers.Base;
using Chatapp.Application.Users.CommandSide.Commands.Upsert;
using Chatapp.Core.Notifications;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Chatapp.API.Controllers
{
    [EnableCors]
    [Route("[controller]")]
    [ApiController]
    public class UserController : ApiController
    {
        public UserController(INotificationHandler<Notification> notifications, IMediator mediator) : base(notifications, mediator)
        {
        }

        [HttpPost]
        [Route("")]
        public async ValueTask<IActionResult> Create([FromBody] UpsertUsersCommand command)
        {
            return await Dispatch(command);
        }

        [HttpGet]
        [Route("")]
        public IActionResult ObterTeste()
        {
            return Ok("Teste");
        }
    }
}
