using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReactWithASP.Server.Models;
using ReactWithASP.Server.Models.InputModels;
using ReactWithASP.Server.Services;
using System.Reflection.Metadata.Ecma335;

namespace ReactWithASP.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IMapper _mapper;
        private readonly AuthServices _authService;
        private readonly ILogger<AuthController> _logger;
        private readonly ConnectionStringsService _connectionStringsService;

        public AuthController(IMapper mapper, AuthServices authServices, ILogger<AuthController> logger, ConnectionStringsService connectionStringsService)
        {
            _authService = authServices;
            _mapper = mapper;
            _logger = logger;
            _connectionStringsService = connectionStringsService;

        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public ActionResult<string> Register(RegisterInputModel userModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (userModel.PasswordHash != userModel.ConfirmedPasswordHash)
                    {
                        return BadRequest("Passwords do not match!");
                    }

                    if (_authService.DoesUserExists(userModel.Email))
                    {
                        return BadRequest("User already exists!");
                    }

                    var mappedModel = _mapper.Map<RegisterInputModel, User>(userModel);
                    mappedModel.Role = "User";

                    var user = _authService.RegisterUser(mappedModel);
                    if (user != null)
                    {
                        var token = _authService.GenerateJwtToken(user.Email, mappedModel.Role, user.UserId);
                        return Ok(token);
                    }
                    return BadRequest("Email or password are not correct!");
                }

                return BadRequest("Invalid or bad request.");
            }
            catch (Exception error)
            {
                _logger.LogError(error.Message);
                return StatusCode(500);

            }
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public ActionResult<string> Login(LoginInputModel userModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_authService.IsAuthenticated(userModel.Email, userModel.PasswordHash))
                    {
                        var user = _authService.GetByEmail(userModel.Email);
                        var token = _authService.GenerateJwtToken(userModel.Email, user.Role, user.UserId);

                        return Ok(token);
                    }

                    return BadRequest("Email or password are not correct!");
                }

                return BadRequest("Invalid or bad request.");

            }
            catch (Exception error)
            {
                _logger.LogError(error.Message);
                return StatusCode(500);
            }
        }
    }
}
