using Chatapp.Core.Notifications;
using Chatapp.Core.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Chatapp.API.Controllers.Base
{
    public abstract class ApiController : ControllerBase
    {
        protected readonly IMediator _mediator;
        private readonly NotificationHandler _notifications;

        public ApiController(INotificationHandler<Notification> notifications,IMediator mediator)
        {
            _notifications = (NotificationHandler)notifications;
            _mediator = mediator;
        }

        protected IEnumerable<Notification> Notifications => _notifications.GetNotifications();

        protected bool IsValidOperation()
        {
            return (!_notifications.HasNotifications());
        }

        protected async ValueTask<IActionResult> Dispatch<TResponse>(IRequest<TResponse> request)
        {
            if (request == null)
            {
                return EmptyRequest();
            }

            return Result(await _mediator.Send(request));
        }

        protected async ValueTask<IActionResult> Dispatch<TResponse>(IRequest<TResponse> request, int statusCodeOnSuccess)
        {
            if (request == null)
            {
                return EmptyRequest();
            }

            return Result(await _mediator.Send(request), statusCodeOnSuccess);
        }

        protected void NotifyError(string code, string message)
        {
            _mediator.Publish(new Notification(code, message));
        }

        protected IActionResult Result<TResponse>(TResponse result, int statusCodeOnSuccess = StatusCodes.Status200OK)
        {
            if (IsValidOperation())
            {
                if (statusCodeOnSuccess == StatusCodes.Status201Created)
                {
                    return Created(string.Empty, result);
                }

                return Ok(result);
            }

            return BadRequest(new
            {
                success = false,
                errors = _notifications.GetNotifications().Select(n => n.Value)
            });
        }

        protected async ValueTask<IActionResult> Dispatch<TResponse>(Func<ValueTask<TResponse>> func)
            => Result(await func.Invoke());

        protected async ValueTask<IActionResult> Dispatch<TResponse>(ValueTask<Result<TResponse>> result)
        {
            var response = await result;

            if (response.IsFailure)
            {
                return BadRequest(new
                {
                    success = false,
                    errors = new List<string> { response.Error }
                });
            }

            return Result(response.Value);
        }

        protected async ValueTask<IActionResult> DispatchFile<TResponse>(IRequest<TResponse> request, string contentType, string fileName)
        {
            return ResultFile(await _mediator.Send(request), contentType, fileName);
        }

        protected IActionResult ResultFile<TResponse>(TResponse result, string contentType, string fileName)
        {
            if (IsValidOperation())
            {
                return File(result as byte[], contentType, fileName);
            }

            return BadRequest(new
            {
                success = false,
                errors = _notifications.GetNotifications().Select(n => n.Value)
            });
        }

        protected IActionResult EmptyRequest()
        {
            return BadRequest(new
            {
                success = false,
                errors = new List<string> { "Informe os dados da requisição" }
            });
        }
    }
}
