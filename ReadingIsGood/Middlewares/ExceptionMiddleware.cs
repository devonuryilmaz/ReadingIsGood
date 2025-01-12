using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ReadingIsGood.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _nextMiddleware;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ExceptionMiddleware(RequestDelegate nextMiddleware, ILogger logger, IWebHostEnvironment webHostEnvironment)
        {
            _nextMiddleware = nextMiddleware;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _nextMiddleware(httpContext);
            }
            catch (Exception ex)
            {
                Logger log = new Logger(_webHostEnvironment);
                log.LogError(ex.ToString());
                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(ex.Message));
            }
        }
    }
}
