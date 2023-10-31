using MediatR;
using SocialMediaApp.Application.Identity.Dtos;
using SocialMediaApp.Application.Models;
using System;
using System.Collections.Generic;

namespace SocialMediaApp.Application.Identity.Commands
{
    public class RegisterIdentity : IRequest<OperationResult<IdentityUserProfileDto>>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string CurrentCity { get; set; }
    }
}
