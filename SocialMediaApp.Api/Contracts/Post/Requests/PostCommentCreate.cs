﻿namespace SocialMediaApp.Api.Contracts.Post.Requests
{
    public record PostCommentCreate
    {
        [Required]
        public string Text { get; set; }

    }
}
