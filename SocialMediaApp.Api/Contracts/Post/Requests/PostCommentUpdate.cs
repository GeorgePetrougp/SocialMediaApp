namespace SocialMediaApp.Api.Contracts.Post.Requests
{
    public record PostCommentUpdate
    {
        [Required]
        public string Text { get; set; }
    }
}
