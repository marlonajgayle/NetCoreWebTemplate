using MediatR;

namespace NetCoreWebTemplate.Application.Identity.Commands.CreateAccount
{
    public class CreateAccountCommand : IRequest<int>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public CreateAccountCommand(CreateAccountDto createAccountDto)
        {
            Email = createAccountDto.Email.ToLower().Trim();
            Password = createAccountDto.Password.Trim();
            ConfirmPassword = createAccountDto.ConfirmPassword.Trim();
        }
    }
}