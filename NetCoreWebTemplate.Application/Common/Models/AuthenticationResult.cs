namespace NetCoreWebTemplate.Application.Common.Models
{
    public class AuthenticationResult
    {
        public string Token { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}