using AutoMapper;
using SocialMediaApp.Application.Identity.Dtos;
using SocialMediaApp.Domain.Aggregates.UserProfileAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Application.Identity.MappingProfiles
{
    public class IdentityProfiles : Profile
    {
        public IdentityProfiles()
        {
            CreateMap<UserProfile, IdentityUserProfileDto>()
                .ForMember(d => d.PhoneNumber, opt => opt.MapFrom(src => src.BasicInfo.PhoneNumber))
                .ForMember(d => d.CurrentCity, opt => opt.MapFrom(src => src.BasicInfo.CurrentCity))
                .ForMember(d => d.EmailAdress, opt => opt.MapFrom(src => src.BasicInfo.EmailAdress))
                .ForMember(d => d.FirstName, opt => opt.MapFrom(src => src.BasicInfo.FirstName))
                .ForMember(d => d.LastName, opt => opt.MapFrom(src => src.BasicInfo.LastName))
                .ForMember(d => d.DateOfBirth, opt => opt.MapFrom(src => src.BasicInfo.DateOfBirth));
        }
    }
}
