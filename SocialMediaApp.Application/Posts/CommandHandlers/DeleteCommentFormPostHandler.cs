using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Application.Enums;
using SocialMediaApp.Application.Models;
using SocialMediaApp.Application.Posts.Commands;
using SocialMediaApp.Data;
using SocialMediaApp.Domain.Aggregates.PostAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Application.Posts.CommandHandlers
{
    public class DeleteCommentFormPostHandler : IRequestHandler<DeleteCommentFromPost, OperationResult<PostComment>>
    {
        private readonly DataContext _context;

        public DeleteCommentFormPostHandler(DataContext context)
        {
            _context = context;  
        }

        public async Task<OperationResult<PostComment>> Handle(DeleteCommentFromPost request,
        CancellationToken cancellationToken)
        {
            var result = new OperationResult<PostComment>();

            try
            {
                var post = await _context.Posts
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.PostId == request.PostId, cancellationToken);


                if (post == null)
                {
                    result.AddError(ErrorCodes.NotFound, string.Format(PostErrorMessages.PostNotFound, request.PostId));
                   
                    return result;
                }

                var comment = post.Comments
                    .FirstOrDefault(c => c.CommentId == request.CommentId);
                if (comment == null)
                {
                    result.AddError(ErrorCodes.NotFound, string.Format(PostErrorMessages.CommentNotFound, request.CommentId));
                    
                    return result;
                }

                if(comment.UserProfileId != request.UserProfileId)
                {
                    result.AddError(ErrorCodes.CommentRemovalNotAuthorized, PostErrorMessages.CommentRemovalNotAuthorised);
                    return result;
                }

                post.RemoveComment(comment);
                _context.Posts.Update(post);
                await _context.SaveChangesAsync(cancellationToken);

                result.Payload = comment;
            }
            catch(Exception ex)
            {
                result.AddUnknownError(ex.Message);
            }
            
            return result;
        }

    }
}
