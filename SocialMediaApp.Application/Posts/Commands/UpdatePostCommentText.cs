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
    public class UpdatePostCommentText : IRequest<OperationResult<PostComment>>
    {
        public Guid UserProfileId { get; set; }
        public Guid PostId { get; set; }
        public Guid CommentId { get; set; }
        public string UpdatedText { get; set; }
    }
}
