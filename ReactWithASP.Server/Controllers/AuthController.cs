using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReactWithASP.Server.Models;
using ReactWithASP.Server.Models.InputModels;
using ReactWithASP.Server.Services;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

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
        public async Task<IActionResult> Register(RegisterInputModel userModel)
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

                    var user = _authService.RegisterUser(userModel);
                    if (user != null)
                    {
                        var cookieOptions = new CookieOptions
                        {
                            HttpOnly = true,
                            Expires = user.TokenExpires,
                            SameSite = SameSiteMode.None,
                            Secure = true,
                            IsEssential = true
                        };

                        Response.Cookies.Append("refreshToken", user.RefreshToken, cookieOptions);

                        await HttpContext.SignInAsync("default", new ClaimsPrincipal(
                           new ClaimsIdentity(
                            new Claim[]
                             {
                                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                                new Claim(ClaimTypes.Role, "User"),
                                new Claim(ClaimTypes.Email, user.Email),
                                new Claim("ID", user.UserId.ToString())
                            },
                                "default"
                            )
                            ),
                            new AuthenticationProperties()
                            {
                                IsPersistent = true,
                            });

                        return Ok("Success! User account has been registered and you are now signed in.");
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
        public async Task<IActionResult> Login(LoginInputModel userModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_authService.IsAuthenticated(userModel.Email, userModel.PasswordHash))
                    {
                        var user = _authService.GetByEmail(userModel.Email);
                        var role = _authService.GetRoleByEmail(userModel.Email);

                        // If true need to generate a new refresh token -- update user account
                        if (user.TokenExpires < DateTime.UtcNow)
                        {
                            user = _authService.UpdateRefreshToken(user);
                        }

                        var cookieOptions = new CookieOptions
                        {
                            HttpOnly = true,
                            Expires = user.TokenExpires,
                            SameSite = SameSiteMode.None,
                            Secure = true,
                            IsEssential = true
                        };

                        Response.Cookies.Append("refreshToken", user.RefreshToken, cookieOptions);

                        await HttpContext.SignInAsync("default", new ClaimsPrincipal(
                           new ClaimsIdentity(
                            new Claim[]
                             {
                                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                                new Claim(ClaimTypes.Role, role),
                                new Claim(ClaimTypes.Email, user.Email),
                                new Claim("ID", user.UserId.ToString())
                            },
                                "default"
                            )
                        ),
                            new AuthenticationProperties()
                            {
                                IsPersistent = true,
                            });
                        return Ok("You have successfully logged in.");
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
        [HttpPost("refresh-token")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            try
            {
                var refreshToken = Request.Cookies["refreshToken"];
                if (refreshToken == null)
                {
                    return BadRequest("Token does not exist or has expired.");
                }

                var accountUser = _authService.GetByRefreshToken(refreshToken);

                if (accountUser == null)
                {
                    return BadRequest("Invalid or bad refresh token.");
                }
                else if (accountUser.TokenExpires < DateTime.UtcNow)
                {
                    return Unauthorized("Token expired. Please log back in.");
                }

                accountUser = _authService.UpdateRefreshToken(accountUser);

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = accountUser.TokenExpires,
                    SameSite = SameSiteMode.None,
                    Secure = true,
                    IsEssential = true
                };
                Response.Cookies.Append("refreshToken", accountUser.RefreshToken, cookieOptions);

                var role = _authService.GetRoleByEmail(accountUser.Email);


                await HttpContext.SignInAsync("default", new ClaimsPrincipal(
                           new ClaimsIdentity(
                            new Claim[]
                             {
                                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                                new Claim(ClaimTypes.Role, role),
                                new Claim(ClaimTypes.Email, accountUser.Email),
                                new Claim("ID", accountUser.UserId.ToString())
                            },
                                "default"
                            )
                        ),
                            new AuthenticationProperties()
                            {
                                IsPersistent = true,
                            });


                return Ok("Success! Session token has been updated.");
            }
            catch (Exception error)
            {
                _logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [AllowAnonymous]
        [HttpGet("Logout")]
        public async Task<ActionResult> SignOutUser()
        {
            try
            {
                await HttpContext.SignOutAsync();

                var cookieOptions = new CookieOptions
                {
                    Secure = true,
                    HttpOnly = true,
                    IsEssential = true
                };

                Response.Cookies.Delete("refreshToken", cookieOptions);

                return Ok("Successfully Signed Out.");
            }
            catch (Exception error)
            {
                _logger.LogError(error.Message);
                return StatusCode(500);
            }
        }
    }
}
