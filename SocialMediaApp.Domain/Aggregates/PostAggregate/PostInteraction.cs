using SocialMediaApp.Domain.Aggregates.UserProfileAggregate;

namespace SocialMediaApp.Domain.Aggregates.PostAggregate
{
    public class PostInteraction
    {
        private PostInteraction()
        {

        }
        public Guid InteractionId { get; private set; }
        public Guid PostId { get; private set; }
        public Guid? UserProfileId { get; private set; }
        public UserProfile UserProfile { get; private set; }
        public InteractionType InteractionType { get; private set; }


        //Factory
        public static PostInteraction CreatePostInteraction(Guid postId,Guid userProfileId, InteractionType interactionType)
        {
            return new PostInteraction
            {
                PostId = postId,
                UserProfileId = userProfileId,
                InteractionType = interactionType

            };

        }

    }
}
