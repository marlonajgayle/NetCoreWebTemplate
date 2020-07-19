using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NetCoreWebTemplate.Application.Common.Interfaces;
using NetCoreWebTemplate.Application.Common.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreWebTemplate.Infrastructure.Identity
{
    public class UserManager : IUserManager
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly JwtSettings jwtSettings;

        public UserManager(UserManager<ApplicationUser> userManager, JwtSettings jwtSettings)
        {
            this.userManager = userManager;
            this.jwtSettings = jwtSettings;
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


        public AuthenticationResult GenerateJwtToken(string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);
            var tokenDiscrptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, email)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials( new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token =  tokenHandler.CreateToken(tokenDiscrptor);

            return new AuthenticationResult
            {
                Success = true,
                Token = tokenHandler.WriteToken(token)
            };
        }

        public async Task<bool> SignInAsync(string email, string password)
        {
            var result = false;
            var user = await userManager.FindByEmailAsync(email);

            if (user != null)
            {
                 result = await userManager.CheckPasswordAsync(user, password);
            }

            return result;
        }
    }
}