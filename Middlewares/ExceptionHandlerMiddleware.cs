using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Review.Api.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            context.Response.StatusCode = 500;
            var response = new { message = e.Message };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}