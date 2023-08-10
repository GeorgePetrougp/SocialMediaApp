using MediatR;
using SocialMediaApp.Application.UserProfiles.Queries;

namespace SocialMediaApp.Api.Registrars
{
    public class NugetServicesRegistrar : IWebApplicationBuilderRegistrar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(typeof(Program), typeof(GetAllUserProfiles));
            builder.Services.AddMediatR(typeof(GetAllUserProfiles));
        }
    }
}
