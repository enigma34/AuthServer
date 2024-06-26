using Asp.Versioning;
using AuthServer.API.DTOs;
using AuthServer.API.Models;
using AuthServer.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace AuthServer.API.Controllers.V1
{
    [ApiVersion(1)]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthController : ControllerBase
    {
        private IUserServices _userServices;
        public AuthController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpPost("register")]
        public ActionResult<RegisterResponse> Register(RegisterRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                bool checkuserexists = _userServices.CheckUserInDb(request.Email);

                if (checkuserexists)
                    return BadRequest("Email id already used");

                string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
                User user = new User();
                user.Id = _userServices.GetUsersCount() + 1;
                user.Email = request.Email;
                user.PasswordHash = passwordHash;
                user.USerRole = request.USerRole;
                _userServices.AddUser(user);
                return Ok(new RegisterResponse { Message= $"User: {user.Id} registered successfully!!!" });
            }
            catch (Exception ex)
            {
                Log.Error("Application error", ex.Message);
                return BadRequest("Internal Server Error");
            }
            
        }

        [HttpPost("login")]
        public ActionResult<LoginResponse> Login(LoginRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                User? user = _userServices.GetUser(request.Email);

                if (user is null)
                    return BadRequest("User not found");

                if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                {
                    return BadRequest("Wrong password");
                }
                LoginResponse token = _userServices.CreateToken(user);
                return Ok(token);
            }
            catch (Exception ex)
            {
                Log.Error("Application error", ex.Message);
                return BadRequest("Internal Server Error");
            }
            
        }

        [HttpPost("RevokeToken"), Authorize(Roles ="Admin")]
        public ActionResult RevokeToken(string token)
        {
            try
            {
                _userServices.RevokeToken(token);
                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error("Application error", ex.Message);
                return BadRequest("Internal Server Error");
            }
        }

        [HttpPost("RefreshToken"), Authorize(Roles = "Admin")]
        public ActionResult<RefreshTokenResponse> RefreshToken(RefreshTokenRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                
                User? user = _userServices.GetUser(request.UserEmail);

                if (user == null)
                    return BadRequest("User not found");

                RefreshToken? userRefreshTpken = _userServices.GetRefreshToken(user.Email);

                if (userRefreshTpken.RefreshTokenExpiryTime <= DateTime.UtcNow || request.RefreshToken != userRefreshTpken.AppRefreshToken)
                    return BadRequest("Invalid token");

                var token = _userServices.CreateToken(user);

                return Ok(new RefreshTokenResponse { NewAccessToken=token.AccessToken, NewRefreshToken = token.RefreshToken});
            }
            catch (Exception ex)
            {
                Log.Error("Application error", ex.Message);
                return BadRequest("Internal Server Error");
            }
        }
    }
}
