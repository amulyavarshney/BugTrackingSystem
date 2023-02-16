using BugTrackingSystem.DAL;
using BugTrackingSystem.Models;
using BugTrackingSystem.ViewModels;
using BugTrackingSystem.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using BugTrackingSystem.Configurations;
using Microsoft.Extensions.Options;

namespace BugTrackingSystem.Services
{
    public class AuthService : IAuthService
    {
        private readonly BugTrackingContext _context;
        private readonly AppSettings _settings;
        public AuthService(BugTrackingContext context, IOptions<AppSettings> options)
        {
            _context = context;
            _settings = options.Value;
        }

        public async Task Register(UserCreateViewModel user)
        {
            // check if the user exists
            if(await UserExistsAsync(user.UserName))
            {
                throw new UserRegistrationFailedException();
            }

            // check if the user role exists
            if(!await RolesExistAsync(user.Roles))
            {
                throw new UserRegistrationFailedException();
            }

            // create password hash and password salt from password
            CreatePasswordHashAndSalt(user.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var userEntity = new User
            {
                Username = user.UserName,
                FullName = user.FullName,
                AvatarUrl = user.AvatarUrl,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
            };

            foreach(var role in user.Roles)
            {
                userEntity.UserRoles.Add(new UserRole
                {
                    User = userEntity,
                    Role = await GetRoleAsync(role)
                });
            }

            await _context.AddAsync(userEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<JwtViewModel> Login(LoginViewModel loginview)
        {
            var userDb = await GetUserAsync(loginview.Username);

            if(!VerifyPasswordHash(loginview.Password, userDb.PasswordHash, userDb.PasswordSalt))
            {
                throw new LoginFailedException();
            }

            return GetToken(userDb);
        }

        private JwtViewModel GetToken(User userDB)
        {
            // create claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userDB.Username),
                new Claim("Fullname", userDB.FullName),
                new Claim("AvatarUrl", userDB.AvatarUrl)
            };

            foreach(var role in GetRoles(userDB))
            {
                claims.Add(new Claim("Roles", role));
            }

            // create symmetric security key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.JwtSecret));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            
            // given the decription of token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(2),
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return new JwtViewModel { Jwt = tokenHandler.WriteToken(securityToken) };
        }
        private IEnumerable<string> GetRoles(User user)
        {
            return user.UserRoles.Select(ur => ur.Role.Name).ToList();
        }
        private async Task<User> GetUserAsync(string username)
        {
            var userDb = await _context.Users
                .Include(user => user.UserRoles)
                .ThenInclude(UserRole => UserRole.Role)
                .FirstOrDefaultAsync(user => user.Username.ToLower() == username.ToLower());
            if (userDb == null)
            {
                throw new LoginFailedException();
            }
            return userDb;
        }

        private async Task<Role> GetRoleAsync(string role)
        {
            var roleDb = await _context.Roles
                .FirstOrDefaultAsync(u => u.Name.ToLower() == role.ToLower());
            if(roleDb == null)
            {
                throw new UserRegistrationFailedException();
            }
            return roleDb;
        }
        private async Task<bool> UserExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username.ToLower() == username.ToLower());
        }

        private async Task<bool> RolesExistAsync(IEnumerable<string> roles)
        {
            foreach(var role in roles)
            {
                var result = await _context.Roles.AnyAsync(r => r.Name.ToLower() == role.ToLower());
                if(!result)
                {
                    return false;
                }
            }
            return true;
        }

        private void CreatePasswordHashAndSalt(string rawPassword, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawPassword));
        }

        private bool VerifyPasswordHash(string rawPassword, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawPassword));

            for(int i=0; i<computeHash.Length; i++)
            {
                if (computeHash[i] != passwordHash[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
