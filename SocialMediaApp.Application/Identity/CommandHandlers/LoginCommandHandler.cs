using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Application.Enums;
using SocialMediaApp.Application.Identity.Commands;
using SocialMediaApp.Application.Identity.Dtos;
using SocialMediaApp.Application.Models;
using SocialMediaApp.Application.Services;
using SocialMediaApp.Data;
using SocialMediaApp.Domain.Aggregates.UserProfileAggregate;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SocialMediaApp.Application.Identity.CommandHandlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, OperationResult<IdentityUserProfileDto>>
    {
        private readonly DataContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IdentityService _identityService;
        private readonly IMapper _mapper;

        public LoginCommandHandler(DataContext context, UserManager<IdentityUser> userManager, IdentityService identityService, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _identityService = identityService;
            _mapper = mapper;
        }
        public async Task<OperationResult<IdentityUserProfileDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<IdentityUserProfileDto>();

            try
            {
                var identityUser = await GetIdentityUserValidatedAsync(request, result);
                if (result.IsError) return result;

                var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(user => user.IdentityId == identityUser.Id);

                result.Payload = _mapper.Map<IdentityUserProfileDto>(userProfile);
                result.Payload.UserName = identityUser.UserName;

                result.Payload.Token = GetJwtString(identityUser, userProfile);

                return result;


            }
            catch (Exception ex)
            {
                result.AddUnknownError(ex.Message);
                
            }
            return result;
        }

        private async Task<IdentityUser> GetIdentityUserValidatedAsync(LoginCommand request, OperationResult<IdentityUserProfileDto> result)
        {
            var identityUser = await _userManager.FindByEmailAsync(request.UserName);

            if (identityUser is null) result.AddError(ErrorCodes.IdentityUserDoesNotExist, IdentityErrorMessages.NonExistantIdentityUser);
            
            var validPass = await _userManager.CheckPasswordAsync(identityUser, request.Password);

            if (!validPass) result.AddError(ErrorCodes.IncorrectPassword, IdentityErrorMessages.InvalidPassword);
           

            return identityUser;
        }

        private string GetJwtString(IdentityUser identityUser, UserProfile user)
        {
            var claimsIdentity = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, identityUser.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Email, identityUser.Email),
                        new Claim("IdentityId", identityUser.Id),
                        new Claim("UserProfileId", user.UserProfileId.ToString())
                    });
            var token = _identityService.CreateSecurityToken(claimsIdentity);

            return _identityService.PrintToken(token);
        }
    }
}
