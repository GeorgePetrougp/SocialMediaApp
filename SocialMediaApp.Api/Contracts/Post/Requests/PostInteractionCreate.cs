namespace SocialMediaApp.Api.Contracts.Post.Requests
{
    public class PostInteractionCreate
    {
        [Required]
        public InteractionType Type { get; set; }
    }
}
