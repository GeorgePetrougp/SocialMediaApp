namespace SocialMediaApp.Api.Contracts.Post.Responses
{
    public record PostCommentResponse
    {
        public Guid CommentId { get; set; }
        public string Text { get; set; }
        public Guid UserProfileId { get; set; }
    }
}
