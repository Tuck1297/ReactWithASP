using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReactWithASP.Server.Models.InputModels;
using ReactWithASP.Server.Models;
using ReactWithASP.Server.Services;

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
        [Authorize]
        public ActionResult<Guid> GetAll()
        {
            try
            {
                var userId = _authServices.DecodeUserIdFromToken(this.Request.Headers["Authorization"]);
                var connectionStrings = _connectionStringsService.GetAllForUser(userId);
                if (connectionStrings != null)
                {
                    return Json(connectionStrings);
                }
                return Json(null);

            }
            catch (Exception error)
            {
                _logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("getById/{csId}", Name = "GetConnectionStringById")]
        [Authorize]
        public ActionResult<Guid> GetById(Guid csId)
        {
            try
            {
                var userId = _authServices.DecodeUserIdFromToken(this.Request.Headers["Authorization"]);
                var connectionString = _connectionStringsService.GetById(csId);

                if (connectionString != null)
                {
                    if (connectionString.UserId == userId)
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
        [Authorize]
        public ActionResult CreateCS(ConnectionStringInputModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = _authServices.DecodeUserIdFromToken(this.Request.Headers["Authorization"]);
                    var mappedModel = _mapper.Map<ConnectionStringInputModel, ConnectionStrings>(model);
                    mappedModel.UserId = userId;
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
        [Authorize]
        public ActionResult UpdateCS(ConnectionStringInputModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = _authServices.DecodeUserIdFromToken(this.Request.Headers["Authorization"]);

                    if (userId != model.UserId)
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

        [HttpDelete("{id}", Name = "DeleteConnectionString")]
        [Authorize]
        public ActionResult DeleteCS(Guid id)
        {
            try
            {
                var userId = _authServices.DecodeUserIdFromToken(this.Request.Headers["Authorization"]);
                var csId = _connectionStringsService.GetById(id);

                if (userId != csId.UserId)
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
