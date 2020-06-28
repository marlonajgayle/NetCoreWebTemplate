using NetCoreWebTemplate.Application.Notifications.Models;
using System.Threading.Tasks;

namespace NetCoreWebTemplate.Application.Common.Interfaces
{
    public interface IMailService
    {
        Task SendMailAsync(MessageDto message);
    }
}
