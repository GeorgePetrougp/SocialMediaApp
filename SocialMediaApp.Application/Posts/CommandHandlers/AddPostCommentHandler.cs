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
                var post = await _context.Posts.FirstOrDefaultAsync(post => post.PostId == request.PostId);

                if (post is null)
                {
                    result.IsError = true;
                    var error = new Error { ErrorCode = ErrorCodes.NotFound, ErrorMessage = $"No User Profile with ID{request.PostId} found" };
                    result.Errors.Add(error);
                    return result;
                }

                var comment = PostComment.CreatePostComment(request.PostId,request.CommentText, request.UserProfileId);

                post.AddComment(comment);

                _context.Posts.Update(post);

                await _context.SaveChangesAsync();

                result.Payload = comment;
            }

            catch (PostCommentNotValidException ex)
            {
                result.IsError = true;
                ex.ValidationErrors.ForEach(e =>
                {
                    var error = new Error { ErrorCode = ErrorCodes.ValidationError, ErrorMessage = $"{ex.Message}" };
                    result.Errors.Add(error);
                });
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
