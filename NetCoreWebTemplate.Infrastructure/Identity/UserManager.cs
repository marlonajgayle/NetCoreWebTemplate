using Microsoft.AspNetCore.Identity;
using NetCoreWebTemplate.Application.Common.Interfaces;
using System.Threading.Tasks;

namespace NetCoreWebTemplate.Infrastructure.Identity
{
    public class UserManager : IUserManager
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UserManager(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IdentityResult> CreateAccount(string email, string password)
        {
            var user = new ApplicationUser
            {
                Email = email,
                UserName = email
            };

            return await userManager.CreateAsync(user, password);
        }

        public async Task<bool> FindByEmailAsync(string email)
        {
            bool result = false;

            var user = await userManager.FindByEmailAsync(email);

            if (user != null)
            {
                result = true;
            }

            return result;
        }
    }
}