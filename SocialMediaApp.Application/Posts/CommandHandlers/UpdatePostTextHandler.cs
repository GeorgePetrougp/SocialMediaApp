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
    public class UpdatePostTextHandler : IRequestHandler<UpdatePostText, OperationResult<Post>>
    {
        private readonly DataContext _context;

        public UpdatePostTextHandler(DataContext context)
        {
            _context = context;
        }
        public async Task<OperationResult<Post>> Handle(UpdatePostText request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<Post>();

            try
            {
                var post = await _context.Posts.FirstOrDefaultAsync(post => post.PostId == request.PostId);

                if(post is null)
                {
                    result.IsError = true;
                    var error = new Error { ErrorCode = ErrorCodes.NotFound, ErrorMessage = $"No User Profile with ID{request.PostId} found" };
                    result.Errors.Add(error);
                    return result;
                }

                if(post.UserProfileId != request.UserProfileId)
                {
                    result.IsError = true;
                    var error = new Error { ErrorCode = ErrorCodes.PostUpdateNotPossible, ErrorMessage = $"Post update not possible.It is not the post owner that initiates the update" };
                    result.Errors.Add(error);
                    return result;
                }

                post.UpdatePostContent(request.NewText);

                await _context.SaveChangesAsync();

                result.Payload = post;
            }
            catch (PostNotValidException ex)
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
