using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Application.Posts
{
    public class PostErrorMessages
    {
        public const string PostNotFound = "No User Profile with ID {0} found";
        public const string PostDeleteNorPossible = "Post delete not possible.Only the owner of the post can delete it";
        public const string PostUpdateNorPossible = "Post update not possible.It is not the post owner that initiates the update";
        public const string CommentNotFound = "Post doesn't include any comment with the specified ID {0}";
    }
}
