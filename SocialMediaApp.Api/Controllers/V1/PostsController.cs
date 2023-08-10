using SocialMediaApp.Api.Contracts.Common;
using SocialMediaApp.Api.Contracts.Post.Requests;
using SocialMediaApp.Api.Contracts.Post.Responses;
using SocialMediaApp.Api.Filters;
using SocialMediaApp.Application.Posts.Commands;
using SocialMediaApp.Application.Posts.Queries;

namespace SocialMediaApp.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route(ApiRoutes.BaseRoute)]
    [ApiController]
    public class PostsController : BaseController
    {

        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public PostsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            var result = await _mediator.Send(new GetAllPosts());

            var mapped = _mapper.Map<List<PostResponse>>(result.Payload);

            return result.IsError ? HandleErrorResponse(result.Errors) : Ok(mapped);
        }

        [HttpGet]
        [Route(ApiRoutes.Posts.IdRoute)]
        [ValidateGuid("id")]
        public async Task<IActionResult> GetById(string id)
        {
            var postId = Guid.Parse(id);
            var query = new GetPostById() { PostId = postId };

            var result = await _mediator.Send(query);
            var mapped = _mapper.Map<PostResponse>(result.Payload);

            return result.IsError ? HandleErrorResponse(result.Errors) : Ok(mapped);

        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreatePost([FromBody] PostCreate newPost)
        {
            var command = new CreatePost
            {
                UserProfileId = Guid.Parse(newPost.UserProfileId),
                TextContent = newPost.TextContent
            };

            var result = await _mediator.Send(command);
            var mapped = _mapper.Map<PostResponse>(result.Payload);

            return result.IsError ? HandleErrorResponse(result.Errors) : CreatedAtAction(nameof(GetById), new { id = result.Payload.UserProfileId }, mapped);
        }

        [HttpPatch]
        [Route(ApiRoutes.Posts.IdRoute)]
        [ValidateGuid("id")]
        [ValidateModel]
        public async Task<IActionResult> UpdatePost([FromBody] PostUpdate updatedPost, string id)
        {
            var command = new UpdatePostText()
            {
                NewText = updatedPost.Text,
                PostId = Guid.Parse(id)
            };

            var result = await _mediator.Send(command);

            return result.IsError ? HandleErrorResponse(result.Errors) : NoContent();


        }

        [HttpDelete]
        [Route(ApiRoutes.Posts.IdRoute)]
        [ValidateGuid("id")]
        public async Task<IActionResult> DeletePost(string id)
        {
            var command = new DeletePost()
            {
                PostId = Guid.Parse(id)
            };
            var result = await _mediator.Send(command);

            return result.IsError ? HandleErrorResponse(result.Errors) : NoContent();
        }

        [HttpGet]
        [Route(ApiRoutes.Posts.PostComments)]
        [ValidateGuid("postId")]
        public async Task<IActionResult> GetCommentsByPostId(string postId)
        {
            var query = new GetPostComments()
            {
                PostId = Guid.Parse(postId)
            };

            var result = await _mediator.Send(query);


            if (result.IsError) HandleErrorResponse(result.Errors);

            var comments = _mapper.Map<List<PostCommentResponse>>(result.Payload);
            return Ok(comments);
        }

        [HttpPost]
        [Route(ApiRoutes.Posts.PostComments)]
        [ValidateGuid("postId")]
        [ValidateModel]
        public async Task<IActionResult> AddCommentToPost(string postId, [FromBody] PostCommentCreate incommingComment)
        {
            var isValid = Guid.TryParse(incommingComment.UserProfileId, out var userProfileId);

            if (!isValid)
            {
                var apiError = new ErrorResponse();

                apiError.StatusCode = 404;
                apiError.StatusPhrase = "Bad Request";
                apiError.TimeStamp = DateTime.Now;
                apiError.Errors.Add("Provided User Profile Id is not in a valid Guid format");

                return BadRequest(apiError);
            }

            var command = new AddPostComment()
            {
                PostId = Guid.Parse(postId),
                UserProfileId = userProfileId,
                CommentText = incommingComment.Text
            };

            var result = await _mediator.Send(command);

            if (result.IsError) return HandleErrorResponse(result.Errors);

            var newComment = _mapper.Map<PostCommentResponse>(result.Payload);

            return Ok(newComment);
        }


        [HttpPatch]
        [Route(ApiRoutes.Posts.PostComments)]
        [ValidateGuid("postId")]
        [ValidateGuid("commentId")]
        public async Task<IActionResult> UpdatePostComment(string postId, string commentId, [FromBody] PostCommentUpdate updatedComment)
        {
            var command = new UpdatePostCommentText()
            {
                UpdatedText = updatedComment.Text,
                PostId = Guid.Parse(postId),
                CommentId = Guid.Parse(commentId)
            };

            var result = await _mediator.Send(command);

            if (result.IsError) return HandleErrorResponse(result.Errors);

            var newComment = _mapper.Map<PostCommentResponse>(result.Payload);

            return Ok(newComment);
        }

        [HttpDelete]
        [Route(ApiRoutes.Posts.CommentById)]
        [ValidateGuid("postId", "commentId")]
        public async Task<IActionResult> DeleteCommentFromPost(string postId, string commentId)
        {

            var command = new DeleteCommentFromPost
            {
                CommentId = Guid.Parse(commentId),
                PostId = Guid.Parse(postId)
            };

            var result = await _mediator.Send(command);

            return result.IsError ? HandleErrorResponse(result.Errors) : NoContent();



        }
    }
}
