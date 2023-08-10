using SocialMediaApp.Api.Contracts.Identity;
using SocialMediaApp.Application.Identity.Commands;

namespace SocialMediaApp.Api.MappingProfiles
{
    public class IdentityMappings : Profile
    {
        public IdentityMappings()
        {
            CreateMap<UserRegistration, RegisterIdentity>();
            CreateMap<UserLogIn, LoginCommand>();
        }
    }
}
