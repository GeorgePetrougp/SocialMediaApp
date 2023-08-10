namespace SocialMediaApp.Api.Contracts.Post.Requests
{
    public record PostCreate
    {
        [Required]
        public string UserProfileId { get; set; }

        [Required]
        [StringLength(1000)]
        public string TextContent { get; set; }
    }
}
