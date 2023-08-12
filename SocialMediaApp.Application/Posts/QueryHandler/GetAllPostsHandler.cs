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
    public class GetAllPostsHandler : IRequestHandler<GetAllPosts, OperationResult<List<Post>>>
    {
        private readonly DataContext _context;
        public GetAllPostsHandler(DataContext context)
        {
            _context = context;
        }
        public async Task<OperationResult<List<Post>>> Handle(GetAllPosts request, CancellationToken cancellationToken)
        {
                var result = new OperationResult<List<Post>>();
            try
            {
                var posts = await _context.Posts.ToListAsync();
                result.Payload = posts;
            }
            catch (Exception e) 
            {

                result.AddUnknownError(e.Message);
                
            }
            return result;
        }
    }
}
