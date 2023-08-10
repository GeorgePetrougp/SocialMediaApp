using SocialMediaApp.Api.Contracts.Common;

namespace SocialMediaApp.Api.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        //overriding the result
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var apiError = new ErrorResponse();
                apiError.StatusCode = 400;
                apiError.StatusPhrase = "Bad Request";
                apiError.TimeStamp = DateTime.Now;
                var errors = context.ModelState.AsEnumerable();

                foreach (var error in errors)
                {
                    foreach(var inner in error.Value.Errors)
                    {
                        apiError.Errors.Add(inner.ErrorMessage);
                    }
                    
                }

                context.Result = new BadRequestObjectResult(apiError);

            }
        }
    }
}
