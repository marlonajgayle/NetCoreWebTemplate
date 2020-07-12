namespace NetCoreWebTemplate.Api.Contracts.Version1.Requests
{
    public class CreateAccountRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
