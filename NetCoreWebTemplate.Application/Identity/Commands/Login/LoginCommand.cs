using MediatR;
using NetCoreWebTemplate.Application.Common.Models;

namespace NetCoreWebTemplate.Application.Identity.Commands.Login
{
    public class LoginCommand : IRequest<AuthenticationResult>
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public LoginCommand(LoginDto loginDto)
        {
            Email = loginDto.Email.ToLower().Trim();
            Password = loginDto.Password.Trim();
        }
    }
}
