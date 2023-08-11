using MediatR;
using SocialMediaApp.Application.Models;
using SocialMediaApp.Domain.Aggregates.PostAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Application.Posts.Commands
{
    public class UpdatePostText : IRequest<OperationResult<Post>>
    {
        public string NewText { get; set; }
        public Guid PostId { get; set; }
        public Guid UserProfileId { get; set; }

    }
}
