using MediatR;
using SocialMediaApp.Application.Enums;
using SocialMediaApp.Application.Models;
using SocialMediaApp.Application.Posts.Commands;
using SocialMediaApp.Application.UserProfiles.Commands;
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
    public class CreatePostHandler : IRequestHandler<CreatePost, OperationResult<Post>>
    {
        private readonly DataContext _context;

        public CreatePostHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<Post>> Handle(CreatePost request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<Post>();
            try
            {
                var post = Post.CreatePost(request.UserProfileId, request.TextContent);
                _context.Posts.Add(post);
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
