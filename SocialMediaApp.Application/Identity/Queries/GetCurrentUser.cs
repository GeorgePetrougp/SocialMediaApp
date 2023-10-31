using MediatR;
using SocialMediaApp.Application.Identity.Dtos;
using SocialMediaApp.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Application.Identity.Queries
{
    public class GetCurrentUser : IRequest<OperationResult<IdentityUserProfileDto>>
    {
        public Guid UserProfileId { get; set; }
        public ClaimsPrincipal ClaimsPrincipal { get; set; }
    }
}
