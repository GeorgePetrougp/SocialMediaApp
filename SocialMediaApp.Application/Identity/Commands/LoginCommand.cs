using MediatR;
using SocialMediaApp.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Application.Identity.Commands
{
    public class LoginCommand : IRequest<OperationResult<string>>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
