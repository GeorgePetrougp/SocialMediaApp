using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Application.Identity
{
    public class IdentityErrorMessages
    {
        public const string NonExistantIdentityUser = "Unable to find user with the specified username";
        public const string InvalidPassword = "Password Not valid. Login Failed";
        public const string EmailAlreadyExists = "Email already exists.Can not Register";
    }
}
