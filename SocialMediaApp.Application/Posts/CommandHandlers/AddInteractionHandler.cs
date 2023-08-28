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
    public class AddInteractionHandler : IRequestHandler<AddInteraction, OperationResult<PostInteraction>>
    {
        private readonly DataContext _context;

        public AddInteractionHandler(DataContext context)
        {
            _context = context;
        }
        public async Task<OperationResult<PostInteraction>> Handle(AddInteraction request, CancellationToken cancellationToken)
        {
            var result  = new OperationResult<PostInteraction>();

            try
            {
                var post = await _context.Posts.Include(p => p.Interactions)
                                               .Where(p => p.PostId == request.PostId)
                                               .FirstOrDefaultAsync(cancellationToken);

                if (post == null)
                {
                    result.AddError(ErrorCodes.NotFound, PostErrorMessages.PostNotFound);
                    return result;

                }

                var interaction = PostInteraction.CreatePostInteraction(request.PostId,request.UserProfileId,request.Type);

                post.AddInteraction(interaction);

                _context.Posts.Update(post);
                await _context.SaveChangesAsync(cancellationToken);

                result.Payload = interaction;

            }
            catch (Exception ex)
            {
                result.AddUnknownError(ex.Message);
            }
            return result;
        }
    }
}
