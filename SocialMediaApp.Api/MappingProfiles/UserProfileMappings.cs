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
            CreateMap<UserProfileCreateUpdate, UpdateUserProfileBasicInfo>();
            CreateMap<UserProfile, UserProfileResponse>();
            CreateMap<BasicInfo, BasicInformation>();
            CreateMap<UserProfile, InteractionUser>()
                .ForMember(d => d.FullName, opt => opt.MapFrom(src => src.BasicInfo.FirstName + " " + src.BasicInfo.LastName))
                .ForMember(d => d.CurrentCity, opt => opt.MapFrom(src => src.BasicInfo.CurrentCity));
        }
    }
}
