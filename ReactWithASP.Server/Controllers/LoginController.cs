using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReactWithASP.Server.Data;
using ReactWithASP.Server.Models;

namespace ReactWithASP.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private DataContext _configuration;

        public LoginController(DataContext config)
        {
            _configuration = config;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            var user = Authenticate(userLogin);
            if (user != null)
            {
                var token = Generate(user);
                return Ok(token);
            }

            return NotFound("User not found or does not exist");
        }

        private string Generate(UserModel user)
        {
            throw new NotImplementedException();
        }

        private UserModel Authenticate(UserLogin userLogin)
        {
            throw new NotImplementedException();
            //var currentUser = _configuration.UserLogin.FirstOrDefault(o => o.Username.ToLower() == userLogin.UserName.ToLower() && o.Password == userLogin.Password);

            //if (currentUser != null)
            //{
            //    return currentUser;
            // }
            //return null;
        }
    }
}
