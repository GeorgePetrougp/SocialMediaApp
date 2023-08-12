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
                await _context.SaveChangesAsync(cancellationToken);

                result.Payload = post;
            }
            catch (PostNotValidException ex)
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
