
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using VHM.Nuget.Serilog.CommandResult;

namespace VHM.Nuget.Serilog.Configurations.ExceptionHandler
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly GlobalException _globalException;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next, GlobalException globalException)
        {
            _next = next;
            _globalException = globalException;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); 
            }
            catch (Exception ex)
            {
                await _globalException.TryHandleAsync(context, ex, CancellationToken.None);

                var result = new Result();
                result.ErroMessage.StatusCode = StatusCodes.Status500InternalServerError;
                result.AddError("Erro interno no servidor", "VHM.Nuget.Serilog");

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var jsonResult = JsonSerializer.Serialize(result);

                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(jsonResult);
            }
        }
    }

}
