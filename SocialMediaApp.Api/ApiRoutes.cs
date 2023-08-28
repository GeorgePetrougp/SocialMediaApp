﻿namespace SocialMediaApp.Api
{
    public class ApiRoutes
    {
        public const string BaseRoute = "api/v{version:apiVersion}/[controller]";

        public class UserProfiles
        {
            public const string IdRoute = "{id}";
        }

        public class Posts
        {
            public const string IdRoute = "{id}";
            public const string PostComments = "{postId}/comments";
            public const string CommentById = "{postId}/comments/{commentId}";
            public const string InteractionById = "{postId}/interactions/{interactionId}";
            public const string PostInteractions = "{postId}/interactions";

        }

        public static class Identity
        {
            public const string Login = "login";
            public const string Registration = "registration";
            public const string IdentityById = "{identityUserId}";
        }
    }
}
