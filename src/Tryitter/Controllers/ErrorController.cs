using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Tryitter.WebApi.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : ControllerBase
{
    [Route("/error")]
    public IActionResult Error()
    {
        var exception = HttpContext.Features?.Get<IExceptionHandlerFeature>()?.Error;

        return exception switch
        {
            BadHttpRequestException => Problem(title: exception.Message, statusCode: 400),
            AggregateException => Problem(title: "Database connection problem", statusCode: 500, detail:exception?.Message),
            SqlException => Problem(title: "Database connection problem", statusCode: 500, detail:exception?.Message),
            _ => Problem(title: "Something went wrong!", statusCode: 500, detail:exception?.Message)
        };
    }
}