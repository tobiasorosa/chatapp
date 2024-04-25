using Chatapp.Core.Results;
using MediatR;

namespace Chatapp.Application.Users.CommandSide.Commands.Upsert
{
    public class UpsertUsersCommand : IRequest<Result<bool, string>>
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public DateTime Birthdate { get; set; }
    }
}
