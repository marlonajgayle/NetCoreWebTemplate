using MediatR;
using NetCoreWebTemplate.Application.Common.Exceptions;
using NetCoreWebTemplate.Application.Common.Interfaces;
using NetCoreWebTemplate.Application.Common.Models;
using System.Threading;
using System.Threading.Tasks;

namespace NetCoreWebTemplate.Application.Identity.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthenticationResult>
    {
        private readonly IUserManager userManager;

        public LoginCommandHandler(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        public async Task<AuthenticationResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Email);

            if (!user)
            {
                throw new UnauthorizedException("User does not exist.");
            }

            var hasValidCreds = await userManager.SignInAsync(request.Email, request.Password);
            if (!hasValidCreds)
            {
                throw new UnauthorizedException("Invalid username and or password.");
            }

            return userManager.GenerateJwtToken(request.Email);           
        }
    }
}
