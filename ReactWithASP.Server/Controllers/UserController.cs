using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReactWithASP.Server.Models;
using ReactWithASP.Server.Models.InputModels;
using ReactWithASP.Server.Models.OutputModels;
using ReactWithASP.Server.Services;

namespace ReactWithASP.Server.Controllers
{
    [ApiController]
    [Route("account")]
    public class UserController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;
        private readonly UserServices _userServices;
        private readonly AuthServices _authServices;

        public UserController(IMapper mapper, ILogger<UserController> logger, UserServices userServices, AuthServices authServices)
        {
            _mapper = mapper;
            _logger = logger;
            _userServices = userServices;
            _authServices = authServices;
        }

        [HttpGet("getAll", Name = "GetAllUserAccountInfo")]
        [Authorize(Policy = "SuperUser-Policy, Admin-Policy")]
        public ActionResult<UserOutputModel[]> GetAll()
        {
            try
            {
                var users = _userServices.getAllUsers();
                if (users != null)
                {
                    return Json(users);
                }
                return Json(null);
            }
            catch (Exception error)
            {
                _logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("getById", Name = "GetUserInfoById")]
        [Authorize(Policy = "SuperUser-Policy, Admin-Policy, User-Policy")]
        public ActionResult<UserOutputModel> getById()
        {
            try
            {
                var userIdClaim = User.FindFirst("ID");
                if (userIdClaim == null)
                {
                    return BadRequest("Invalid or bad request");
                }
                var user = _userServices.GetById(Guid.Parse(userIdClaim.Value));
                if (user != null)
                {
                    return Json(user);
                }
                return NotFound();
            }
            catch (Exception error)
            {
                _logger.LogError(error.Message);
                return StatusCode(500);
            }

        }

        [HttpPut("update", Name = "UpdateUserInfo")]
        [Authorize(Policy = "SuperUser-Policy, Admin-Policy, User-Policy")]
        public ActionResult Update(UpdateUserModel model)
        {
            try
            {
                var userIdClaim = User.FindFirst("ID");
                if (userIdClaim == null)
                {
                    return BadRequest("Invalid or bad request");
                }
                var updatedUser = _userServices.Update(Guid.Parse(userIdClaim.Value), model);
                if (updatedUser != null)
                {
                    // if this is true then need to generate a new jwt and refresh token
                    return Ok("Success!");
                }
                return BadRequest("Invalid or bad request");
            }
            catch (Exception error)
            {
                _logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpDelete("delete/{email}", Name = "DeleteUserInfoByEmail")]
        [Authorize(Policy = "SuperUser-Policy, Admin-Policy")]
        public ActionResult Delete(string email)
        {
            try
            {
                var user = _authServices.GetByEmail(email);
                if (user != null)
                {
                    _userServices.Delete(user);
                    return Ok("Success!");
                }
                return BadRequest("Invalid or bad request");
            }
            catch (Exception error)
            {
                _logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpPut("roleUpdate", Name = "UpdateUserRoleByEmail")]
        [Authorize(Policy = "SuperUser-Policy, Admin-Policy")]
        public ActionResult UpdateRole(RoleUpdateModelInput model)
        {
            try
            {
                var user = _authServices.GetByEmail(model.Email);
                if (user != null)
                {
                    _userServices.UpdateRole(model.Email, model.Role);
                    return Ok("Success!");
                }
                return BadRequest("Invalid or bad request");
            }
            catch (Exception error)
            {
                _logger.LogError(error.Message);
                return StatusCode(500);
            }
        }
    }
}
