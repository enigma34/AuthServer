using AuthServer.API.Services;
using Serilog;

namespace AuthServer.API.Middleware
{
    public class TokenRevocationMiddleware
    {
        private readonly RequestDelegate _next;
        private IUserServices _userServices;
        public TokenRevocationMiddleware(RequestDelegate next, IUserServices userServices)
        {
            _next = next;
            _userServices = userServices;
        }

      
        public async Task Invoke(HttpContext context)
        {
            try
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (token != null && _userServices.IsTokenRevoked(token))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Token has been revoked.");
                    return;
                }

                await _next(context);
            }
            catch (Exception ex)
            {
                Log.Error("Application error", ex.Message);
            }
        }
        
    }
}
