using SocialMediaApp.Api.Contracts.Common;
using SocialMediaApp.Application.Enums;
using SocialMediaApp.Application.Models;

namespace SocialMediaApp.Api.Controllers.V1
{
    public class BaseController : ControllerBase
    {
        protected IActionResult HandleErrorResponse(List<Error> errors)
        {
            var apiError = new ErrorResponse();

            if (errors.Any(error => error.ErrorCode == ErrorCodes.NotFound))
            {
                var error = errors.FirstOrDefault(error => error.ErrorCode == ErrorCodes.NotFound);
                apiError.StatusCode = 404;
                apiError.StatusPhrase = "NotFound";
                apiError.TimeStamp = DateTime.Now;
                apiError.Errors.Add(error.ErrorMessage);

                return NotFound(apiError);
            }            

                apiError.StatusCode = 500;
                apiError.StatusPhrase = "Internal server error";
                apiError.TimeStamp = DateTime.Now;
                apiError.Errors.Add("Unknown Error");
                return StatusCode(500, apiError);
            
        }
    }
}
