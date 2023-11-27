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
        [Authorize(Roles = "SuperUser,Admin")]
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
        [Authorize]
        public ActionResult<UserOutputModel> getById()
        {
            try
            {
                var userId = _authServices.DecodeUserIdFromToken(this.Request.Headers["Authorization"]);
                var user = _userServices.GetById(userId);
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
        [Authorize]
        public ActionResult Update(UpdateUserModel model)
        {
            try
            {
                var userId = _authServices.DecodeUserIdFromToken(this.Request.Headers["Authorization"]);
                var mappedModel = _mapper.Map<UpdateUserModel, User>(model);
                var updatedUser = _userServices.Update(userId, mappedModel);
                if (updatedUser != null)
                {
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

        [HttpDelete("delete/{email}", Name ="DeleteUserInfo")]
        [Authorize(Roles = "Admin, SuperUser")]
        public ActionResult Delete(string email)
        {
            try
            {
                var user = _authServices.GetByEmail(email);
                if (user != null)
                {
                    _userServices.Delete(user.UserId);
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
