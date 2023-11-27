using Microsoft.IdentityModel.Tokens;
using ReactWithASP.Server.Data;
using ReactWithASP.Server.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Emit;
using System.Security.Claims;
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
                Expires = DateTime.UtcNow.AddMinutes(5),
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
            var user = _dataContext.Users.FirstOrDefault(x => x.Email == email);
            return user != null;
        }

        public User GetById(Guid id)
        {
            return _dataContext.Users.FirstOrDefault(c => c.UserId == id);
        }

        public User GetByEmail(string email)
        {
            return _dataContext.Users.FirstOrDefault(c => c.Email == email);
        }

        public User RegisterUser(User model)
        {
            var id = new Guid();
            var existWithId = this.GetById(id);

            while (existWithId != null)
            {
                id = new Guid();
                existWithId = this.GetById(id);
            }

            model.UserId = id;
            model.PasswordHash = BC.HashPassword(model.PasswordHash);
            var userEntity = _dataContext.Users.Add(model);
            _dataContext.SaveChanges();
        
            return userEntity.Entity;
        }

        public string DecodeEmailFromToken(string token)
        {
            var decodedToken = new JwtSecurityTokenHandler();
            var indexOfTokenValue = 7;
            var t = decodedToken.ReadJwtToken(token.Substring(indexOfTokenValue));

            return (string)t.Payload.FirstOrDefault(x => x.Key == "email").Value;

        }

        public Guid DecodeUserIdFromToken(string token)
        {
            var decodedToken = new JwtSecurityTokenHandler();
            var indexOfTokenValue = 7;
            var t = decodedToken.ReadJwtToken(token.Substring(indexOfTokenValue));

            var stringId = t.Payload.FirstOrDefault(x => x.Key == "UserId").Value.ToString();

            return Guid.Parse(stringId);
        }

        public string DecodeRoleFromToken(string token)
        {
            var decodedToken = new JwtSecurityTokenHandler();
            var indexOfTokenValue = 7;
            var t = decodedToken.ReadJwtToken(token.Substring(indexOfTokenValue));

            return (string)t.Payload.FirstOrDefault(x => x.Key == "Role").Value;
        }
    }
}
