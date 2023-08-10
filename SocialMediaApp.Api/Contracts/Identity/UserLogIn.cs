
namespace SocialMediaApp.Api.Contracts.Identity
{
    public record UserLogIn
    {
        [EmailAddress]
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
