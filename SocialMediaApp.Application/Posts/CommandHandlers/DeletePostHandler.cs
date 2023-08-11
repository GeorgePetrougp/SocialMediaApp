using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Application.Enums;
using SocialMediaApp.Application.Models;
using SocialMediaApp.Application.Posts.Commands;
using SocialMediaApp.Data;
using SocialMediaApp.Domain.Aggregates.PostAggregate;


namespace SocialMediaApp.Application.Posts.CommandHandlers
{
    public class DeletePostHandler : IRequestHandler<DeletePost, OperationResult<Post>>
    {
        private readonly DataContext _context;

        public DeletePostHandler(DataContext context)
        {
            _context = context;
        }
        public async Task<OperationResult<Post>> Handle(DeletePost request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<Post>();

            try
            {
                var post = await _context.Posts.FirstOrDefaultAsync(post => post.PostId == request.PostId);

                if (post is null)
                {
                    result.IsError = true;
                    var error = new Error { ErrorCode = ErrorCodes.NotFound, ErrorMessage = $"No User Profile with ID{request.PostId} found" };
                    result.Errors.Add(error);
                    return result;
                }

                if(post.UserProfileId != request.UserProfileId)
                {
                    result.IsError = true;
                    var error = new Error { ErrorCode = ErrorCodes.PostDeleteNotPossible, ErrorMessage = $"Post delete not possible.Only the owner of the post can delete it" };
                    result.Errors.Add(error);
                    return result;
                }

                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();

                result.Payload = post;

            }
            catch (Exception ex)
            {
                var error = new Error { ErrorCode = ErrorCodes.UnknownError, ErrorMessage = $"{ex.Message}" };
                result.IsError = true;
                result.Errors.Add(error);
            }

            return result;
        }
    }
}
