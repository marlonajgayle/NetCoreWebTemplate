namespace NetCoreWebTemplate.Infrastructure.Notifications.Email
{
    public class EmailConfigurations
    {
        public string FromAddress { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsRequireSsl { get; set; }
    }
}
