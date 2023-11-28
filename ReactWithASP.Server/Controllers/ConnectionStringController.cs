using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReactWithASP.Server.Models.InputModels;
using ReactWithASP.Server.Models;
using ReactWithASP.Server.Services;
using System.Security.Claims;

namespace ReactWithASP.Server.Controllers
{
    [ApiController]
    [Route("cs")]
    public class ConnectionStringController : Controller
    {

        private readonly IMapper _mapper;
        private readonly ILogger<ConnectionStringController> _logger;
        private readonly ConnectionStringsService _connectionStringsService;
        private readonly AuthServices _authServices;

        public ConnectionStringController(IMapper mapper, ILogger<ConnectionStringController> logger, ConnectionStringsService connectionStringService, AuthServices authServices)
        {
            _mapper = mapper;
            _logger = logger;
            _connectionStringsService = connectionStringService;
            _authServices = authServices;

        }

        [HttpGet("getAll", Name = "GetAllConnectionStrings")]
        [Authorize(Policy = "User-Policy")]
        [Authorize(Policy = "Admin-Policy")]
        [Authorize(Policy = "SuperUser-Policy")]
        public ActionResult<Guid> GetAll()
        {
            try
            {
                var userIdClaim = User.FindFirst("ID");
                if (userIdClaim == null)
                {
                    return BadRequest("Invalid or bad request");
                }
                var connectionStrings = _connectionStringsService.GetAllForUser(Guid.Parse(userIdClaim.Value));
                if (connectionStrings != null)
                {
                    return Json(connectionStrings);
                }
                return Json("Empty");

            }
            catch (Exception error)
            {
                _logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("getById/{csId}", Name = "GetConnectionStringById")]
        [Authorize(Policy = "SuperUser-Policy, Admin-Policy, User-Policy")]
        public ActionResult<Guid> GetById(Guid csId)
        {
            try
            {
                var userIdClaim = User.FindFirst("ID");
                if (userIdClaim == null)
                {
                    return BadRequest("Invalid or bad request");
                }
                var connectionString = _connectionStringsService.GetById(csId);

                if (connectionString != null)
                {
                    if (connectionString.UserId == Guid.Parse(userIdClaim.Value))
                    {
                        return Json(connectionString);
                    }
                    return BadRequest("Invalid or bad request.");
                }

                return Json(null);
            }
            catch (Exception error)
            {
                _logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpPost("create", Name = "CreateConnectionString")]
        [Authorize(Policy = "SuperUser-Policy, Admin-Policy, User-Policy")]
        public ActionResult CreateCS(ConnectionStringInputModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userIdClaim = User.FindFirst("ID");
                    if (userIdClaim == null)
                    {
                        return BadRequest("Invalid or bad request");
                    }
                    var mappedModel = _mapper.Map<ConnectionStringInputModel, ConnectionStrings>(model);
                    mappedModel.UserId = Guid.Parse(userIdClaim.Value);
                    var csEntity = _connectionStringsService.Create(mappedModel);
                    if (csEntity != null)
                    {
                        return Ok("Success!");
                    }
                }
                return BadRequest("Invalid or bad request.");
            }
            catch (Exception error)
            {
                _logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpPut("update", Name = "UpdateConnectionString")]
        [Authorize(Policy = "SuperUser-Policy, Admin-Policy, User-Policy")]
        public ActionResult UpdateCS(ConnectionStringInputModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userIdClaim = User.FindFirst("ID");
                    if (userIdClaim == null)
                    {
                        return BadRequest("Invalid or bad request");
                    }

                    if (Guid.Parse(userIdClaim.Value) != model.UserId)
                    {
                        return BadRequest("Invalid or bad request.");
                    }

                    var mappedModel = _mapper.Map<ConnectionStringInputModel, ConnectionStrings>(model);
                    var updatedCS = _connectionStringsService.Update(model.Id, mappedModel);
                    if (updatedCS != null)
                    {
                        return Ok("Success!");
                    }
                }
                return BadRequest("Invalid or bad request.");
            }
            catch (Exception error)
            {
                _logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpDelete("delete/{id}", Name = "DeleteConnectionString")]
        [Authorize(Policy = "SuperUser-Policy, Admin-Policy, User-Policy")]
        public ActionResult DeleteCS(Guid id)
        {
            try
            {
                var userIdClaim = User.FindFirst("ID");
                if (userIdClaim == null)
                {
                    return BadRequest("Invalid or bad request");
                }
                var csId = _connectionStringsService.GetById(id);

                if (Guid.Parse(userIdClaim.Value) != csId.UserId)
                {
                    return BadRequest("Invalid or bad request");
                }

                if (csId != null)
                {
                    _connectionStringsService.Delete(id);
                    return Ok("Success!");
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
