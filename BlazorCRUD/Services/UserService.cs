using BlazorCRUD.Models;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;
using static BlazorCRUD.Pages.Register;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BlazorCRUD.Services
{
    public class UserService : IUserService
    {
        private readonly IDbService _dbService;
        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();
        public UserService(IDbService dbService)
        {
            _dbService = dbService;

        }

        public async Task<(bool Success, string ErrorMessage)> RegisterUser(RegisterModel model)
        {
            var existingUser = await _dbService.Get<User>("SELECT * FROM users WHERE email = @Email", new { Email = model.Email });
            if (existingUser != null)
            {
                return (false, "User already exists.");
            }

            var user = new User { Email = model.Email };
            user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);

            var result = await _dbService.Insert<int>("INSERT INTO users (email, password_hash) VALUES (@Email, @PasswordHash)", user);

            return (true, string.Empty);
        }

        private string HashPassword(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashed;
        }

        public async Task<(bool Success, string ErrorMessage, User User)> AuthenticateUser(string email, string password)
        {
            var user = await _dbService.Get<User>("SELECT * FROM users WHERE email = @Email", new { Email = email });
            if (user == null)
            {
                return (false, "User not found.", null);
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (result == PasswordVerificationResult.Failed)
            {
                return (false, "Invalid password.", null);
            }

            return (true, string.Empty, user);
        }
        public string GenerateJwtToken(User user)
        {
            var keyString = "YourVeryLongSecureSecretKeyHere"; 
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("sub", user.Email) }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }


    }

}
