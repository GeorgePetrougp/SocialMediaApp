using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Domain.Exceptions
{
    public class UserProfileNotValidException : NotValidException
    {
        internal UserProfileNotValidException() { }
        internal UserProfileNotValidException(string message) : base(message) { }
        internal UserProfileNotValidException(string message, Exception inner) : base(message, inner) { }
    }
}
