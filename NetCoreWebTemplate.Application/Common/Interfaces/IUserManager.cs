using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace NetCoreWebTemplate.Application.Common.Interfaces
{
    public interface IUserManager
    {
        /// <summary>
        ///  Create Identity Account for user.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<IdentityResult> CreateAccount(string email, string password);

        /// <summary>
        /// Find if user account exists
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<bool> FindByEmailAsync(string email);
    }
}