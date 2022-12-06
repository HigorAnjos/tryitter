using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Tryitter.WebApi.Filters;

public class ErrorHandlerFilterAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        var exception = context.Exception;

        var problemDetails = new ProblemDetails
        {
            Title = exception.Message,
            Status = (int)HttpStatusCode.InternalServerError
        };
        
        context.Result = new ObjectResult(problemDetails);

        context.ExceptionHandled = true;
    }
}