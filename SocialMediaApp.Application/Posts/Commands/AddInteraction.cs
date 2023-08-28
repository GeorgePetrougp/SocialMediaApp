using MediatR;
using SocialMediaApp.Application.Models;
using SocialMediaApp.Domain.Aggregates.PostAggregate;

namespace SocialMediaApp.Application.Posts.Commands
{
    public class AddInteraction : IRequest<OperationResult<PostInteraction>>
    {
        public Guid PostId { get; set; }
        public Guid UserProfileId { get; set; }
        public InteractionType Type { get; set; }
    }
}
