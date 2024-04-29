using Chatapp.Core.Entities;
using Chatapp.Core.Results;

namespace Chatapp.Application.Users.CommandSide.Entities
{
    public class User : Entity
    {
        protected User()
        {
        }

        private User(string userName, string email, string password, DateTime birthdate)
        {
            UserName = userName;
            Email = email;
            Password = password;
            Birthdate = birthdate;
        }

        public string UserName { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public DateTime Birthdate { get; private set; }

        public static Result<User> Criar(string userName, string email, string password, DateTime birthdate)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return Result<User>.Failure("Nome de usuário é obrigatório");
            }
            if (userName.Length > 50)
            {
                return Result<User>.Failure("Nome de usuário deve ter menos que 50 caracteres");
            }

            if (string.IsNullOrEmpty(email))
            {
                return Result<User>.Failure("Email é obrigatório");
            }
            if (email.Length > 60)
            {
                return Result<User>.Failure("Email deve ter menos que 60 caracteres");
            }

            if (string.IsNullOrEmpty(password))
            {
                return Result<User>.Failure("Senha é obrigatória");
            }
            if (userName.Length > 50)
            {
                return Result<User>.Failure("Senha deve ter menos que 50 caracteres");
            }

            var obj = new User(userName, email, password, birthdate);

            return Result<User>.Success(obj);
        }
    }
}
