using SocialMediaApp.Domain.Exceptions;
using SocialMediaApp.Domain.Validators.UserProfileValidators;

namespace SocialMediaApp.Domain.Aggregates.UserProfileAggregate
{
    public class BasicInfo
    {
        private BasicInfo() { }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string EmailAdress { get; private set; }
        public string PhoneNumber { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public string CurrentCity { get; private set; }


        // Factory

        /// <summary>
        /// Creates a new BasicInfo instance
        /// </summary>
        /// <param name="firstName">First name</param>
        /// <param name="lastName">Last name</param>
        /// <param name="emailAddress">Emnail address</param>
        /// <param name="phone">Phone</param>
        /// <param name="dateOfBirth">Date of Birth</param>
        /// <param name="currentCity">Current city</param>
        /// <returns><see cref="BasicInfo"/></returns>
        /// <exception cref="UserProfileNotValidException"></exception>
        public static BasicInfo CreateBasicInfo(string firstName, string lastName, string emailAddress, string phoneNumber, DateTime dateOfBirth, string currentCity)
        {
            var validator = new BasicInfoValidator();

            var objectToValidate = new BasicInfo
            {
                FirstName = firstName,
                LastName = lastName,
                EmailAdress = emailAddress,
                PhoneNumber = phoneNumber,
                DateOfBirth = dateOfBirth,
                CurrentCity = currentCity
            };

            var validationResult = validator.Validate(objectToValidate);

            if (validationResult.IsValid) return objectToValidate;

            var exception = new UserProfileNotValidException("The user profile in not valid");

            foreach(var error in validationResult.Errors)
            {
                exception.ValidationErrors.Add(error.ErrorMessage);
            }

            throw exception;
        }

    }
}
