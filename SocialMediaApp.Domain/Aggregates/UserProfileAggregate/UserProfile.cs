namespace SocialMediaApp.Domain.Aggregates.UserProfileAggregate
{
    public class UserProfile
    {
        private UserProfile()
        {

        }
        public Guid UserProfileId { get; private set; }
        public string IdentityId { get; private set; }
        public BasicInfo BasicInfo { get; private set; }
        public DateTime DateCreated { get; private set; }
        public DateTime LastModified { get; private set; }

        // Factory
        public static UserProfile CreateUserProfile(string identityId, BasicInfo basicInfo)
        {
            return new UserProfile()
            {
                IdentityId = identityId,
                BasicInfo = basicInfo,
                DateCreated = DateTime.Now,
                LastModified = DateTime.Now
            };

        }

        // Public Methods

        public void UpdateBasicInfo(BasicInfo newInfo)
        {
            BasicInfo = newInfo;
            LastModified = DateTime.Now;
        }

    }
}
