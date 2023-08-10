using AutoMapper;
using SocialMediaApp.Api.Contracts.Post.Responses;
using SocialMediaApp.Domain.Aggregates.PostAggregate;

namespace SocialMediaApp.Api.MappingProfiles
{
    public class PostMappings : Profile
    {
        public PostMappings()
        {
            CreateMap<Post, PostResponse>();
            CreateMap<PostComment, PostCommentResponse>();

        }
    }
}
