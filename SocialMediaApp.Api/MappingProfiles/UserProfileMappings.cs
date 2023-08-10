using AutoMapper;
using SocialMediaApp.Api.Contracts.UserProfile.Requests;
using SocialMediaApp.Api.Contracts.UserProfile.Responses;
using SocialMediaApp.Application.UserProfiles.Commands;
using SocialMediaApp.Domain.Aggregates.UserProfileAggregate;

namespace SocialMediaApp.Api.MappingProfiles
{
    public class UserProfileMappings : Profile
    {
        public UserProfileMappings()
        {
            CreateMap<UserProfileCreateUpdate, CreateUserCommand>();
            CreateMap<UserProfileCreateUpdate, UpdateUserProfileBasicInfo>();
            CreateMap<UserProfile, UserProfileResponse>();
            CreateMap<BasicInfo, BasicInformation>();
        }
    }
}
