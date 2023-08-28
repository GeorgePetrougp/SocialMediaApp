
using PostInteraction = SocialMediaApp.Domain.Aggregates.PostAggregate.PostInteraction;


namespace SocialMediaApp.Api.MappingProfiles
{
    public class PostMappings : Profile
    {
        public PostMappings()
        {
            CreateMap<Post, PostResponse>();
            CreateMap<PostComment, PostCommentResponse>();

            CreateMap<PostInteraction,Contracts.Post.Responses.PostInteraction>()
                .ForMember(d => d.Type, 
                    opt => opt.MapFrom(
                        src => src.InteractionType.ToString()))
                .ForMember(d => d.Author, opt => opt.MapFrom(src => src.UserProfile));


        }
    }
}
