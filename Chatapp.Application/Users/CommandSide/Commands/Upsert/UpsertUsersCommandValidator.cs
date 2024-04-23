using FluentValidation;

namespace Chatapp.Application.Users.CommandSide.Commands.Upsert
{
    public sealed class UpsertUsersCommandValidator : AbstractValidator<UpsertUsersCommand>
    {
        public UpsertUsersCommandValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .NotEmpty();

            RuleFor(x => x.UserName)
                .MaximumLength(50)
                .NotEmpty();

            RuleFor(x => x.Birthdate)
                .Must(BeValidDate)
                .NotEmpty();
        }

        protected static bool BeValidDate(DateTime date)
        {
            DateTime oldDate = DateTime.Today.AddYears(-120);

            return date > oldDate;
        }
    }
}
