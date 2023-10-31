using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Application.Enums;
using SocialMediaApp.Application.Identity.Dtos;
using SocialMediaApp.Application.Identity.Queries;
using SocialMediaApp.Application.Models;
using SocialMediaApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Application.Identity.QueryHandlers
{
    public class GetCurrentUserHandler : IRequestHandler<GetCurrentUser, OperationResult<IdentityUserProfileDto>>
    {
        private readonly DataContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;

        public GetCurrentUserHandler(DataContext context, UserManager<IdentityUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<OperationResult<IdentityUserProfileDto>> Handle(GetCurrentUser request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<IdentityUserProfileDto>();

            var identity = await _userManager.GetUserAsync(request.ClaimsPrincipal);

            var profile = await _context.UserProfiles.FirstOrDefaultAsync(up => up.UserProfileId == request.UserProfileId, cancellationToken);

            result.Payload = _mapper.Map<IdentityUserProfileDto>(profile);
            result.Payload.UserName = identity.UserName;

            return result;


        }
    }
}
