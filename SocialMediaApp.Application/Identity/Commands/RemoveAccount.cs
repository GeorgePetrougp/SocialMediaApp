using MediatR;
using SocialMediaApp.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Application.Identity.Commands
{
    public class RemoveAccount : IRequest<OperationResult<bool>>
    {
        public Guid IdentityUserId { get; set; }
        public Guid RequesterGuid { get; set; }
    }
}
