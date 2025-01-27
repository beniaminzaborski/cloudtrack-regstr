using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace CloudTrack.Registration.WebAPI.ExceptionsHandling;

internal class GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger) : IExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _logger = logger;

    public void OnException(ExceptionContext context)
    {
        _logger.LogError(new EventId(context.Exception.HResult),
                context.Exception,
                context.Exception.Message);

        var errorObjectResult = ErrorObjectResult.Create(context.Exception);
        context.Result = errorObjectResult;
        context.HttpContext.Response.StatusCode = errorObjectResult.StatusCode ?? (int)HttpStatusCode.InternalServerError;
        context.ExceptionHandled = true;
    }
}
