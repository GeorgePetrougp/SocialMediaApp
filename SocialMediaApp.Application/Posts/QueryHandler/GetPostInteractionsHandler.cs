using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Application.Enums;
using SocialMediaApp.Application.Models;
using SocialMediaApp.Application.Posts.Queries;
using SocialMediaApp.Data;
using SocialMediaApp.Domain.Aggregates.PostAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Application.Posts.QueryHandler
{
    public class GetPostInteractionsHandler : IRequestHandler<GetPostInteractions, OperationResult<List<PostInteraction>>>
    {

        private readonly DataContext _context;

        public GetPostInteractionsHandler(DataContext context)
        {
            _context = context;
        }
        public async Task<OperationResult<List<PostInteraction>>> Handle(GetPostInteractions request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<List<PostInteraction>>();

            try
            {
                var post = await _context.Posts.Include(p => p.Interactions)
                                               .ThenInclude(i => i.UserProfile)
                                                       .Where(p => p.PostId == request.PostId)
                                                       .FirstOrDefaultAsync(cancellationToken);

                if (post == null)
                {
                    result.AddError(ErrorCodes.NotFound,PostErrorMessages.PostNotFound);
                    return result;

                }

                result.Payload = post.Interactions.ToList();

            }
            catch (Exception ex)
            {
                result.AddUnknownError(ex.Message);

            }
            return result;
        }
    }
}
