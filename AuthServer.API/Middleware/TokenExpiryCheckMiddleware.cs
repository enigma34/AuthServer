using Serilog;
using System.IdentityModel.Tokens.Jwt;

namespace AuthServer.API.Middleware
{
    public class TokenExpiryCheckMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenExpiryCheckMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (token != null)
                {
                    var handler = new JwtSecurityTokenHandler();
                    if (handler.CanReadToken(token))
                    {
                        var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
                        var expiry = jwtToken?.ValidTo;

                        if (expiry.HasValue)
                        {
                            Console.WriteLine($"Token expires at: {expiry.Value.ToUniversalTime()}");
                        }

                        if (expiry.HasValue && expiry.Value < DateTime.UtcNow)
                        {
                            context.Response.StatusCode = 401;
                            await context.Response.WriteAsync("Token has expired.");
                            return;
                        }
                    }
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
