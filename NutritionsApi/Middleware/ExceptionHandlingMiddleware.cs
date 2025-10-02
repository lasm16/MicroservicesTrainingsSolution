using Microsoft.AspNetCore.Mvc;
using NutritionsApi.Exceptions;

namespace NutritionsApi.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context, ILogger<ExceptionHandlingMiddleware> logger)
    {
        try
        {
            await next(context);
        }
        catch (NotFoundException ex)
        {
            logger.LogError(ex, "Object not found.");
            context.Response.StatusCode = 404;
            await context.Response.WriteAsJsonAsync(new{error = ex.Message});
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error executing request.");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new{error = ex.Message});
        }
    } 
}