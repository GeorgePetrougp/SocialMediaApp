using SocialMediaApp.Api.Contracts.Identity;
using SocialMediaApp.Application.Identity.Commands;
using SocialMediaApp.Application.Identity.Dtos;

namespace SocialMediaApp.Api.MappingProfiles
{
    public class IdentityMappings : Profile
    {
        public IdentityMappings()
        {
            CreateMap<UserRegistration, RegisterIdentity>();
            CreateMap<UserLogIn, LoginCommand>();
            CreateMap<IdentityUserProfileDto, IdentityUserProfile>();
        }
    }
}
