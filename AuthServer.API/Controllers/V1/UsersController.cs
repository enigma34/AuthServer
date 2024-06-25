using Asp.Versioning;
using AuthServer.API.Models;
using AuthServer.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace AuthServer.API.Controllers.V1
{
    [ApiVersion(1)]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserServices _userServices;
        public UsersController( IUserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpGet(Name = "GetListOfUsers")]
        public IActionResult GetAll()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                return Ok(_userServices.GetAllUsers());
            }
            catch (Exception ex)
            {
                Log.Error("Application error", ex.Message);
                return BadRequest("Internal Server Error");
            }            
        }
    }
}
