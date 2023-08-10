namespace SocialMediaApp.Api.Contracts.Common
{
    public record ErrorResponse
    {
        public int StatusCode { get; set; }
        public string StatusPhrase{ get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public DateTime TimeStamp { get; set; }
    }
}
