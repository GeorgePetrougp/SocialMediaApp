namespace SocialMediaApp.Api.Contracts.Post.Responses
{
    public record PostResponse
    {
        public Guid PostId { get;  set; }
        public Guid UserProfileId { get;  set; }
        public string TextContent { get;  set; }
        public DateTime CreatedDate { get;  set; }
        public DateTime LastModified { get;  set; }
    }
}
