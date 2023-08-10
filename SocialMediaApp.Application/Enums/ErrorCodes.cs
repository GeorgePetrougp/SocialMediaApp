using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Application.Enums
{
    public enum ErrorCodes
    {
        NotFound = 404,
        ServerError = 500,

        //Validation errors should be in the range 100 - 199
        ValidationError = 101,
        FriendRequestValidationError = 102,

        //Infrastructure errors should be in the range 200-299
        IdentityCreationFailed = 202,
        DatabaseOperationException = 203,

        //Application errors should be in the range 300 - 399
        PostUpdateNotPossible = 300,
        PostDeleteNotPossible = 301,
        InteractionRemovalNotAuthorized = 302,
        IdentityUserAlreadyExists = 303,
        IdentityUserDoesNotExist = 304,
        IncorrectPassword = 305,
        UnauthorizedAccountRemoval = 306,
        CommentRemovalNotAuthorized = 307,
        FriendRequestAcceptNotPossible = 308,
        FriendRequestRejectNotPossible = 309,


        UnknownError = 999
    }
}
