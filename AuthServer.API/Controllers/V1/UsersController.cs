﻿using Asp.Versioning;
using AuthServer.API.Models;
using AuthServer.API.Services;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UsersController( IUserServices userServices, IHttpContextAccessor httpContextAccessor)
        {
            _userServices = userServices;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet(Name = "GetListOfUsers"),Authorize(Roles = "Admin")]
        public IActionResult GetAll()
        {
            try
            {
                //if (!ModelState.IsValid)
                //{
                //    return BadRequest(ModelState);
                //}
               // var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                //var checkTokenExpiry = _userServices.ValidateToken(token);
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
