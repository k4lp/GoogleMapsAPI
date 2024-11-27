using System.Net;

namespace GoogleMapsAPI.Middlewares.ExceptionHandlingMiddlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                _logger.Log(LogLevel.Information, httpContext.Request.Body
                    .ToString());
                await _next(httpContext);
                _logger.Log(LogLevel.Information, httpContext.Response.Body.ToString());
            }
            catch (Exception ex)
            {
                await HandleException(httpContext, ex);
            }
        }

        private static Task HandleException(HttpContext httpContext, Exception exception)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            httpContext.Response.ContentType = "application/json";
#pragma warning disable CS8604 // Possible null reference argument.
            return httpContext.Response.WriteAsync(new
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = "Internal Server Error"
            }.ToString());
#pragma warning restore CS8604 // Possible null reference argument.

        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
