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
                    result.AddError(ErrorCodes.NotFound, string.Format(PostErrorMessages.PostNotFound,request.PostId));
                    return result;
                }

                if(post.UserProfileId != request.UserProfileId)
                {
                    result.AddError(ErrorCodes.PostDeleteNotPossible, PostErrorMessages.PostDeleteNorPossible);
                    
                    return result;
                }

                _context.Posts.Remove(post);
                await _context.SaveChangesAsync(cancellationToken);

                result.Payload = post;

            }
            catch (Exception ex)
            {
                result.AddUnknownError(ex.Message);
            }

            return result;
        }
    }
}
