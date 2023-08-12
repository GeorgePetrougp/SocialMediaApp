using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Application.Enums;
using SocialMediaApp.Application.Models;
using SocialMediaApp.Application.Posts.Commands;
using SocialMediaApp.Data;
using SocialMediaApp.Domain.Aggregates.PostAggregate;
using SocialMediaApp.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Application.Posts.CommandHandlers
{
    public class UpdatePostCommentTextHandler : IRequestHandler<UpdatePostCommentText, OperationResult<PostComment>>
    {
        private readonly DataContext _ctx;

        public UpdatePostCommentTextHandler(DataContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<OperationResult<PostComment>> Handle(UpdatePostCommentText request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PostComment>();

            try
            {
                var post = await _ctx.Posts
                    .Include(p => p.Comments)
                    .FirstOrDefaultAsync(p => p.PostId == request.PostId);

                if (post is null)
                {
                    result.AddError(ErrorCodes.NotFound, string.Format(PostErrorMessages.PostNotFound, request.PostId));
                    
                    return result;
                }

                var comment = post.Comments.FirstOrDefault(c => c.CommentId == request.CommentId);

                if (comment is null)
                {
                    result.AddError(ErrorCodes.NotFound, string.Format(PostErrorMessages.CommentNotFound, request.CommentId));
                    return result;
                }

                comment.UpdateCommentText(request.UpdatedText);

                await _ctx.SaveChangesAsync();

                result.Payload = comment;
            }
            catch (PostCommentNotValidException e)
            {


                e.ValidationErrors.ForEach(error => result.AddError(ErrorCodes.ValidationError, error));

            }
            catch (Exception e)
            {
                result.AddUnknownError(e.Message);
            }

            return result;
        }
    }
}
