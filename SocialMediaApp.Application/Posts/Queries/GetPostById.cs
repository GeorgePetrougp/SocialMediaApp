using MediatR;
using SocialMediaApp.Application.Models;
using SocialMediaApp.Domain.Aggregates.PostAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Application.Posts.Queries
{
    public class GetPostById : IRequest<OperationResult<Post>>
    {
        public Guid PostId { get; set; }
    }
}
