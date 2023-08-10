using SocialMediaApp.Domain.Aggregates.UserProfileAggregate;
using SocialMediaApp.Domain.Exceptions;
using SocialMediaApp.Domain.Validators.PostValidators;

namespace SocialMediaApp.Domain.Aggregates.PostAggregate
{
    public class Post
    {
        private readonly List<PostComment> _comments = new List<PostComment>();
        private readonly List<PostInteraction> _interactions = new List<PostInteraction>();
        private Post()
        {
        }
        public Guid PostId { get; private set; }
        public Guid UserProfileId { get; private set; }
        public UserProfile UserProfile { get; private set; }
        public string TextContent { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public DateTime LastModified { get; private set; }
        public IEnumerable<PostComment> Comments { get { return _comments;  } }
        public IEnumerable<PostInteraction> Interactions { get { return _interactions;  } }

        //Factory

        /// <summary>
        /// Creates a new post instance
        /// </summary>
        /// <param name="userProfileId">User profile ID</param>
        /// <param name="textContent">Post content</param>
        /// <returns><see cref="Post"/></returns>
        /// <exception cref="PostNotValidException"></exception>
        public static Post CreatePost(Guid userProfileId, string textContent)
        {
            var validator = new PostValidator();
            var objectToValidate = new Post
            {
                UserProfileId = userProfileId,
                TextContent = textContent,
                CreatedDate = DateTime.UtcNow,
                LastModified = DateTime.UtcNow,
            };

            var validationResult = validator.Validate(objectToValidate);

            if (validationResult.IsValid) return objectToValidate;

            var exception = new PostNotValidException("Post is not valid");
            validationResult.Errors.ForEach(vr => exception.ValidationErrors.Add(vr.ErrorMessage));
            throw exception;
        }

        //Public Methods

        /// <summary>
        /// Updates the post text
        /// </summary>
        /// <param name="newText">The updated post text</param>
        /// <exception cref="PostNotValidException"></exception>
        public void UpdatePostContent(string newText)
        {
            if (string.IsNullOrWhiteSpace(newText))
            {
                var exception = new PostNotValidException("Connot update post.Text is not valid ");
                exception.ValidationErrors.Add("The text is either null or contains only space");
                throw exception;
            }
            TextContent = newText;
            LastModified = DateTime.Now;
        }

        public void AddComment(PostComment newComment)
        {
            _comments.Add(newComment);
        }
        public void RemoveComment(PostComment commentToRemove)
        {
            _comments.Remove(commentToRemove);
        }

        public void AddInteraction(PostInteraction newInteraction)
        {
            _interactions.Add(newInteraction);
        }
        public void RemoveInteraction(PostInteraction interactionToRemove)
        {
            _interactions.Remove(interactionToRemove);
        }


    }
}
