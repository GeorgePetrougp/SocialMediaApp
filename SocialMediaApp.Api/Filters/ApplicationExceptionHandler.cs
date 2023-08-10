using SocialMediaApp.Api.Contracts.Common;
using System.Text.Json.Nodes;

namespace SocialMediaApp.Api.Filters
{
    public class ApplicationExceptionHandler : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var apiError = new ErrorResponse();
            apiError.StatusCode = 500;
            apiError.StatusPhrase = "Internal Server Error";
            apiError.TimeStamp = DateTime.Now;
            apiError.Errors.Add(context.Exception.Message);

            context.Result = new JsonResult(apiError) { StatusCode = 500 };
        }
    }
}
