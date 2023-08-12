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
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Application.Posts.QueryHandler
{
    public class GetPostCommentsHandler : IRequestHandler<GetPostComments, OperationResult<List<PostComment>>>
    {
        private readonly DataContext _context;

        public GetPostCommentsHandler(DataContext context)
        {
            _context = context;
        }
        public async Task<OperationResult<List<PostComment>>> Handle(GetPostComments request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<List<PostComment>>();

            try
            {
                var post = await _context.Posts.Include(post => post.Comments).FirstOrDefaultAsync(post => post.PostId == request.PostId);

                result.Payload = post.Comments.ToList();

            }
            catch (Exception e)
            {
                result.AddUnknownError(e.Message);
            }    
            return result;

        }
    }
}
