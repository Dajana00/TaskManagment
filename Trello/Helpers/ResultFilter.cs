using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Trello.Filters
{
    public class ResultFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult objectResult &&
                objectResult.Value is IResult result)
            {
                if (result.ToResult().IsSuccess)
                {
                    base.OnActionExecuted(context);
                    return;
                }

                var firstError = result.ToResult().Errors.FirstOrDefault();

                int statusCode = firstError switch
                {
                    Error e when e.Message.Contains("not found", StringComparison.OrdinalIgnoreCase) => (int)HttpStatusCode.NotFound,
                    Error e when e.Message.Contains("unauthorized", StringComparison.OrdinalIgnoreCase) => (int)HttpStatusCode.Unauthorized,
                    Error e when e.Message.Contains("forbidden", StringComparison.OrdinalIgnoreCase) => (int)HttpStatusCode.Forbidden,
                    Error e when e.Message.Contains("invalid", StringComparison.OrdinalIgnoreCase) => (int)HttpStatusCode.BadRequest,
                    _ => (int)HttpStatusCode.BadRequest
                };

                var problemDetails = new ProblemDetails
                {
                    Status = statusCode,
                    Title = "Request failed",
                    Detail = firstError?.Message,
                    Instance = context.HttpContext.Request.Path
                };

                context.Result = new ObjectResult(problemDetails)
                {
                    StatusCode = statusCode
                };
            }

            base.OnActionExecuted(context);
        }
    }
}
