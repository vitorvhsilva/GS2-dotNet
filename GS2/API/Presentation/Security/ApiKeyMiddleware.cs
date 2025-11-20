namespace API .Presentation.Security;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private const string APIKEY_HEADER = "x-api-key";

    public ApiKeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
    {
        if (!context.Request.Headers.TryGetValue(APIKEY_HEADER, out var extractedApiKey))
        {
            context.Response.StatusCode = 401; 
            await context.Response.WriteAsync("API Key não informada.");
            return;
        }

        var apiKey = configuration["ApiSettings:ApiKey"];

        if (!apiKey.Equals(extractedApiKey))
        {
            context.Response.StatusCode = 403; 
            await context.Response.WriteAsync("API Key inválida.");
            return;
        }

        await _next(context);
    }
}
