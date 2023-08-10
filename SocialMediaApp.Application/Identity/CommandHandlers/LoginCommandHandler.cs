using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SocialMediaApp.Application.Enums;
using SocialMediaApp.Application.Identity.Commands;
using SocialMediaApp.Application.Models;
using SocialMediaApp.Application.Options;
using SocialMediaApp.Data;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Application.Identity.CommandHandlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, OperationResult<string>>
    {
        private readonly DataContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;

        public LoginCommandHandler(DataContext context, UserManager<IdentityUser> userManager, IOptions<JwtSettings> jwtSettings)
        {
            _context = context;
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
        }
        public async Task<OperationResult<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<string>();

            try
            {
                var identityUser = await _userManager.FindByEmailAsync(request.UserName);

                if(identityUser is null)
                {
                    result.IsError = true;
                    var error = new Error { ErrorCode = ErrorCodes.IdentityUserDoesNotExist, ErrorMessage = $"Unable to find user with the specified username" };
                    result.Errors.Add(error);
                    return result;
                }

                var validPass = await _userManager.CheckPasswordAsync(identityUser, request.Password);

                if (!validPass)
                {
                    result.IsError = true;
                    var error = new Error { ErrorCode = ErrorCodes.IncorrectPassword, ErrorMessage = $"Password Not valid. Login Failed" };
                    result.Errors.Add(error);
                    return result;
                }

                var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(user => user.IdentityId == identityUser.Id);

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtSettings.SigningKey);
                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, identityUser.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Email, identityUser.Email),
                        new Claim("IdentityId", identityUser.Id),
                        new Claim("UserProfileId", userProfile.UserProfileId.ToString())
                    }),
                    Expires = DateTime.Now.AddHours(2),
                    Audience = _jwtSettings.Audiences[0],
                    Issuer = _jwtSettings.Issuer,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                result.Payload = tokenHandler.WriteToken(token);
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
    }
}
