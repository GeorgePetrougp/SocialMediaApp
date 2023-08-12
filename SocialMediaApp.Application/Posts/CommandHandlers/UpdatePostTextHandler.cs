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
                    result.AddError(ErrorCodes.NotFound, string.Format(PostErrorMessages.PostNotFound, request.PostId));
                    
                    return result;
                }

                if(post.UserProfileId != request.UserProfileId)
                {
                    result.AddError(ErrorCodes.PostUpdateNotPossible, PostErrorMessages.PostUpdateNorPossible);

                    return result;
                }

                post.UpdatePostContent(request.NewText);

                await _context.SaveChangesAsync();

                result.Payload = post;
            }
            catch (PostNotValidException ex)
            {
                ex.ValidationErrors.ForEach(e => result.AddError(ErrorCodes.ValidationError,e));
            }
            catch (Exception ex)
            {
                result.AddUnknownError(ex.Message);
            }

            return result;

        }
    }
}
