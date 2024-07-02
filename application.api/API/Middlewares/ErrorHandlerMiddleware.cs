using System.Net;

namespace API.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode code;
            string result;

            switch (exception)
            {
                case UnauthorizedAccessException _:
                    code = HttpStatusCode.Unauthorized;
                    result = System.Text.Json.JsonSerializer.Serialize(new { error = "Unauthorized access." });
                    break;
                case FileNotFoundException _:
                    code = HttpStatusCode.NotFound;
                    result = System.Text.Json.JsonSerializer.Serialize(new { error = "File not found." });
                    break;
                default:
                    code = HttpStatusCode.InternalServerError;
                    result = System.Text.Json.JsonSerializer.Serialize(new { error = "An unexpected error occurred. Please try again later." });
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
