using Microsoft.IdentityModel.Tokens;
using ReactWithASP.Server.Data;
using ReactWithASP.Server.Models;
using ReactWithASP.Server.Models.InputModels;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Emit;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BC = BCrypt.Net.BCrypt;

namespace ReactWithASP.Server.Services
{
    public class AuthServices
    {
        private readonly IConfiguration _configuration;
        private readonly DataContext _dataContext;

        public AuthServices(IConfiguration configuration, DataContext dataContext)
        {
            _configuration = configuration;
            _dataContext = dataContext;
        }
        public string GenerateJwtToken(string email, string role, Guid userId)
        {
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var jwtKey = _configuration["Jwt:Key"];

            Console.WriteLine($"Issuer: {issuer}");
            Console.WriteLine($"Audience: {audience}");
            Console.WriteLine($"JWT Key: {jwtKey}");

            if (string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience) || string.IsNullOrEmpty(jwtKey))
            {
                throw new InvalidOperationException("JWT configuration values are missing or empty.");
            }

            var key = Encoding.ASCII.GetBytes(jwtKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                            new Claim("Id", Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.Sub, email),
                            new Claim(JwtRegisteredClaimNames.Email, email),
                            new Claim(ClaimTypes.Role, role),
                            new Claim("UserId", userId.ToString()),
                            new Claim(JwtRegisteredClaimNames.Jti,
                            Guid.NewGuid().ToString())
                        }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        public bool IsAuthenticated(string email, string password)
        {
            var user = this.GetByEmail(email);
            return this.DoesUserExists(email) && BC.Verify(password, user.PasswordHash);
        }

        public bool DoesUserExists(string email)
        {
            var user = _dataContext.UserAccount.FirstOrDefault(x => x.Email.ToLower() == email.ToLower());
            return user != null;
        }

        public UserAccount GetById(Guid id)
        {
            return _dataContext.UserAccount.FirstOrDefault(c => c.UserId == id);
        }

        public UserAccount GetByEmail(string email)
        {
            return _dataContext.UserAccount.FirstOrDefault(c => c.Email.ToLower() == email.ToLower());
        }

        public UserAccount GetByRefreshToken(string refreshToken)
        {
            return _dataContext.UserAccount.FirstOrDefault(c => c.RefreshToken == refreshToken);
        }

        public string GetRoleByEmail(string email)
        {
            var user = _dataContext.Users.FirstOrDefault(c => c.Email == email);
            if (user != null)
            {
                return user.Role;
            }
            return null;
        }

        public UserAccount RegisterUser(RegisterInputModel model)
        {
            var id = Guid.NewGuid();

            var existWithId = this.GetById(id);

            while (existWithId != null)
            {
                id = Guid.NewGuid();
                existWithId = this.GetById(id);
            }
            var userAccount = new UserAccount{
                UserId = id,
                Email = model.Email.ToLower(),
                PasswordHash = BC.HashPassword(model.PasswordHash),
                RefreshToken = GenerateRefreshToken(),
                TokenCreated = DateTime.UtcNow,
                TokenExpires = DateTime.UtcNow.AddDays(7)
            };

            var user = new User
            {
                UserId = id,
                Email = model.Email.ToLower(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                Role = "User"
            };


            var userAccountEntity = _dataContext.UserAccount.Add(userAccount);
            var userEntity = _dataContext.Users.Add(user);
            _dataContext.SaveChanges();
        
            return  userAccountEntity.Entity;
        }

        public UserAccount UpdateRefreshToken(UserAccount model)
        {
            model.RefreshToken = GenerateRefreshToken();
            model.TokenCreated = DateTime.UtcNow;
            model.TokenExpires = DateTime.UtcNow.AddDays(7);
            _dataContext.SaveChanges();
            return model;
        }

        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

    }
}
