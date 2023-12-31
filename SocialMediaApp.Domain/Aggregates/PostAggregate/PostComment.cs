﻿using SocialMediaApp.Domain.Exceptions;
using SocialMediaApp.Domain.Validators.PostValidators;

namespace SocialMediaApp.Domain.Aggregates.PostAggregate
{
    public class PostComment
    {
        private PostComment()
        {

        }
        public Guid CommentId { get; private set; }
        public Guid PostId { get; private set; }
        public string Text { get; private set; }
        public Guid UserProfileId { get; private set; }
        public DateTime DateCreated { get; private set; }
        public DateTime LastModified { get; private set; }
        public ICollection<PostComment> Comments { get; private set; }


        //Factory

        /// <summary>
        /// Creates a post comment
        /// </summary>
        /// <param name="postId">The ID of the post to which the comment belongs</param>
        /// <param name="text">Text content of the comment</param>
        /// <param name="userProfileId">The ID of the user who created the comment</param>
        /// <returns><see cref="PostComment"/></returns>
        /// <exception cref="PostCommentNotValidException">Thrown if the data provided for the post comment
        /// is not valid</exception>

        public static PostComment CreatePostComment(Guid postId, string text, Guid userProfileId)
        {
            var validator =  new PostCommentValidator();
            var objectToValidate = new PostComment
            {
                PostId = postId,
                Text = text,
                UserProfileId = userProfileId,
                DateCreated = DateTime.Now,
                LastModified = DateTime.Now

            };

            var validationResult = validator.Validate(objectToValidate);

            if (validationResult.IsValid) return objectToValidate;

            var exception = new PostCommentNotValidException("Post comment is not valid");

            validationResult.Errors.ForEach(result => exception.ValidationErrors.Add(result.ErrorMessage));
            throw exception;
        }

        //Public Methods

        /// <summary>
        /// Updated the text content of a comment
        /// </summary>
        /// <param name="newText">The text content of the updated comment</param>
        /// <exception cref="PostCommentNotValidException"></exception>

        public void UpdateCommentText(string newText)
        {
            if (string.IsNullOrWhiteSpace(newText))
            {
                var exception = new PostCommentNotValidException("Cannot update comment." + "Comment text content is not valid");

                exception.ValidationErrors.Add("The provided text is either null or contains only white space");
                throw exception;
            }

            Text = newText;
            LastModified = DateTime.Now;
        }
    }
}
