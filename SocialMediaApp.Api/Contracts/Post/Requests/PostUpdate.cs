namespace SocialMediaApp.Api.Contracts.Post.Requests
{
    public record PostUpdate
    {
        [Required]
        public string Text { get; set; }
    }
}
