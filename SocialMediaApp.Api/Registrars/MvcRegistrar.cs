using Microsoft.AspNetCore.Mvc.Versioning;
using SocialMediaApp.Api.Filters;

namespace SocialMediaApp.Api.Registrars
{
    public class MvcRegistrar : IWebApplicationBuilderRegistrar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers(config =>
            {
                config.Filters.Add(typeof(ApplicationExceptionHandler));
            });

            //Versioning Configuration
            builder.Services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0); //Specify Default Api Version
                config.AssumeDefaultVersionWhenUnspecified = true; // for not returning errors(using default version)when clients make calls where they do not specify version
                config.ReportApiVersions = true;//to each response that you create automatically adds a header that will tell which are the supported api versions
                config.ApiVersionReader = new UrlSegmentApiVersionReader();//reading/handling this version use this approach to have the versioning in the url and not in headers
            });

            //Swager with versioning
            builder.Services.AddVersionedApiExplorer(config =>
            {
                config.GroupNameFormat = "'v'VVV";
                config.SubstituteApiVersionInUrl = true;// dynamic url in controller
            });


            builder.Services.AddEndpointsApiExplorer();

        }
    }
}
