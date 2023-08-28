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
    public class RemovePostInteractionHandler : IRequestHandler<RemovePostInteraction, OperationResult<PostInteraction>>
    {

        private readonly DataContext _context;

        public RemovePostInteractionHandler(DataContext context)
        {
            _context = context;
        }
        public async Task<OperationResult<PostInteraction>> Handle(RemovePostInteraction request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PostInteraction>();

            try
            {
                var post = await  _context.Posts.Include(post => post.Interactions)
                                         .Where(p => p.PostId == request.PostId)
                                         .FirstOrDefaultAsync(cancellationToken);

                if(post == null)
                {
                    result.AddError(ErrorCodes.NotFound, string.Format(PostErrorMessages.PostNotFound, request.PostId));
                    return result;
                }

                var interaction = post.Interactions.Where(i => i.InteractionId == request.InteractionId)
                                                   .FirstOrDefault();

                if(interaction == null)
                {
                    result.AddError(ErrorCodes.NotFound, PostErrorMessages.PostInteractionNotFound);
                    return result;
                }

                if(interaction.UserProfileId != request.UserProfileId)
                {
                    result.AddError(ErrorCodes.InteractionRemovalNotAuthorized, PostErrorMessages.InteractionRemovalNotAuthorised);
                    return result;
                }

                post.RemoveInteraction(interaction);
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
