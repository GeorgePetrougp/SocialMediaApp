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
    public class AddPostCommentHandler : IRequestHandler<AddPostComment, OperationResult<PostComment>>
    {
        private readonly DataContext _context;

        public AddPostCommentHandler(DataContext context)
        {
            _context = context;
        }
        public async Task<OperationResult<PostComment>> Handle(AddPostComment request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PostComment>();

            try
            {
                var post = await _context.Posts.FirstOrDefaultAsync(post => post.PostId == request.PostId, cancellationToken);

                if (post is null)
                {
                    result.AddError(ErrorCodes.NotFound, string.Format(PostErrorMessages.PostNotFound, request.PostId));
                    return result;
                }

                var comment = PostComment.CreatePostComment(request.PostId,request.CommentText, request.UserProfileId);

                post.AddComment(comment);

                _context.Posts.Update(post);

                await _context.SaveChangesAsync(cancellationToken);

                result.Payload = comment;
            }

            catch (PostCommentNotValidException ex)
            {
                ex.ValidationErrors.ForEach(e => result.AddError(ErrorCodes.ValidationError, e));
            }

            catch (Exception ex)
            {
                result.AddUnknownError(ex.Message);
            }

            return result;
        }
    }
}
