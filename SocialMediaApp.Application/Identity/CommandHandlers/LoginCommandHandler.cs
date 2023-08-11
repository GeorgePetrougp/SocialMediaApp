using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Application.Enums;
using SocialMediaApp.Application.Identity.Commands;
using SocialMediaApp.Application.Models;
using SocialMediaApp.Application.Services;
using SocialMediaApp.Data;
using SocialMediaApp.Domain.Aggregates.UserProfileAggregate;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SocialMediaApp.Application.Identity.CommandHandlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, OperationResult<string>>
    {
        private readonly DataContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IdentityService _identityService;

        public LoginCommandHandler(DataContext context, UserManager<IdentityUser> userManager, IdentityService identityService)
        {
            _context = context;
            _userManager = userManager;
            _identityService = identityService;
        }
        public async Task<OperationResult<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<string>();

            try
            {
                var identityUser = await GetIdentityUserValidatedAsync(request, result);
                if (identityUser == null) return result;

                var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(user => user.IdentityId == identityUser.Id);


                result.Payload = GetJwtString(identityUser, userProfile);

                return result;


            }
            catch (Exception ex)
            {

                var error = new Error { ErrorCode = ErrorCodes.UnknownError, ErrorMessage = $"{ex.Message}" };
                result.IsError = true;
                result.Errors.Add(error);
            }
            return result;
        }

        private async Task<IdentityUser> GetIdentityUserValidatedAsync(LoginCommand request, OperationResult<string> result)
        {
            var identityUser = await _userManager.FindByEmailAsync(request.UserName);

            if (identityUser is null)
            {
                result.IsError = true;
                var error = new Error { ErrorCode = ErrorCodes.IdentityUserDoesNotExist, ErrorMessage = $"Unable to find user with the specified username" };
                result.Errors.Add(error);
                return null;
            }

            var validPass = await _userManager.CheckPasswordAsync(identityUser, request.Password);

            if (!validPass)
            {
                result.IsError = true;
                var error = new Error { ErrorCode = ErrorCodes.IncorrectPassword, ErrorMessage = $"Password Not valid. Login Failed" };
                result.Errors.Add(error);
                return null;
            }

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
