using Chatapp.Core.Commands;
using Chatapp.Core.Notifications;
using Chatapp.Core.Repositories;
using Chatapp.Core.Results;
using MediatR;

namespace Chatapp.Application.Users.CommandSide.Commands.Upsert
{
    public class UpsertUsersCommandHandler : BaseCommandHandler, IRequestHandler<UpsertUsersCommand, Result<bool, string>>
    {
        public UpsertUsersCommandHandler(IUnitOfWork uow, IMediator mediator, INotificationHandler<Notification> notifications) : base(uow, mediator, notifications)
        {
        }

        public async Task<Result<bool, string>> Handle(UpsertUsersCommand command, CancellationToken cancellationToken)
        {
            return Result<bool, string>.Success();
        }
    }
}
