using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Application.Enums;
using SocialMediaApp.Application.Models;
using SocialMediaApp.Application.Posts.Queries;
using SocialMediaApp.Data;
using SocialMediaApp.Domain.Aggregates.PostAggregate;
using SocialMediaApp.Domain.Aggregates.UserProfileAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Application.Posts.QueryHandler
{
    public class GetPostByIdHandler : IRequestHandler<GetPostById, OperationResult<Post>>
    {
        private readonly DataContext _context;

        public GetPostByIdHandler(DataContext context)
        {
            _context = context;
        }
        public async Task<OperationResult<Post>> Handle(GetPostById request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<Post>();
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.PostId == request.PostId);

            if (post is null)
            {
                result.AddError(ErrorCodes.NotFound, string.Format(PostErrorMessages.PostNotFound, request.PostId));
                return result;
            }

            result.Payload = post;

            return result;

        }
    }
}
