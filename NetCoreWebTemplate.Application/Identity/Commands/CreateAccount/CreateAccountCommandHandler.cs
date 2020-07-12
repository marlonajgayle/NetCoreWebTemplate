using MediatR;
using NetCoreWebTemplate.Application.Common.Exceptions;
using NetCoreWebTemplate.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace NetCoreWebTemplate.Application.Identity.Commands.CreateAccount
{
    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, int>
    {
        private readonly IUserManager userManager;

        public CreateAccountCommandHandler(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        public async Task<int> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (!user)
            {
                throw new BadRequestException("Sorry, that email address has already registered.");
            }

            var result = await userManager.CreateAccount(request.Email, request.Password);
            if (!result.Succeeded)
            {
                throw new BadRequestException("Unable to create account, due to errors!");
            }

            return 0;
        }
    }
}