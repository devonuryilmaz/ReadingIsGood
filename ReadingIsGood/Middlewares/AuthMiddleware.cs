using Microsoft.AspNetCore.Http.Features;
using ReadingIsGood.Business.Services.Abstract;
using ReadingIsGood.Business.Services.Interface;

namespace ReadingIsGood.API.Middlewares
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _nextMiddleWare;

        public AuthMiddleware(RequestDelegate next)
        {
            _nextMiddleWare = next;
        }


        public async Task InvokeAsync(HttpContext httpContext, IUserService userService, ITokenService tokenService)
        {
            var endpoint = httpContext.Features.Get<IEndpointFeature>()?.Endpoint;
            if (endpoint != null)
            {
                var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                var userId = tokenService.ValidateToken(token);
                if (userId != null)
                {
                    // attach user to context on successful jwt validation
                    httpContext.Items["User"] = await userService.GetById(userId.Value);
                }
            }
            await _nextMiddleWare(httpContext);
        }
    }
}
