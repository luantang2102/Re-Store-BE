using System;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace API.Middleware;

public class ExceptionMiddleware(IHostEnvironment env, ILogger<ExceptionMiddleware> logger) : IMiddleware  
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
      try
      {
        await next(context);
      }
      catch (System.Exception e) 
      {
        await HandleException(context, e);
      }
    } 
      
    private async Task HandleException(HttpContext context, Exception e)
    {
      logger.LogError(e, e.Message);
      context.Response.ContentType = "application/json";
      context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

      var response = new ProblemDetails
      {
        Status = 500,
        Detail = env.IsDevelopment() ? e.StackTrace?.ToString() : null,
        Title = e.Message
      };

      var option = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
      var json = JsonSerializer.Serialize(response, option);

      await context.Response.WriteAsync(json);
    }
}
