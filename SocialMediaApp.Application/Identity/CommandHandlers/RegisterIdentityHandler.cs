using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SocialMediaApp.Application.Enums;
using SocialMediaApp.Application.Identity.Commands;
using SocialMediaApp.Application.Models;
using SocialMediaApp.Application.Options;
using SocialMediaApp.Data;
using SocialMediaApp.Domain.Aggregates.UserProfileAggregate;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SocialMediaApp.Application.Identity.CommandHandlers
{
    public class RegisterIdentityHandler : IRequestHandler<RegisterIdentity, OperationResult<string>>
    {
        private readonly DataContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;

        public RegisterIdentityHandler(DataContext context, UserManager<IdentityUser> userManager, IOptions<JwtSettings> jwtSettings)
        {
            _context = context;
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
        }
        public async Task<OperationResult<string>> Handle(RegisterIdentity request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<string>();
            try
            {
                var existingIdentity = await _userManager.FindByEmailAsync(request.UserName);

                if (existingIdentity != null)
                {
                    result.IsError = true;
                    var error = new Error { ErrorCode = ErrorCodes.IdentityUserAlreadyExists, ErrorMessage = $"Email already exists.Can not Register" };
                    result.Errors.Add(error);
                    return result;
                }

                var identity = new IdentityUser
                {
                    UserName = request.UserName,
                    Email = request.UserName
                };

                    using var transaction = _context.Database.BeginTransaction();// so that If something fails inside this transaction to rollback and not do anything included in the using!
                    var createdIdentity = await _userManager.CreateAsync(identity, request.Password);
                    if (!createdIdentity.Succeeded)
                    {
                        await transaction.RollbackAsync();

                        result.IsError = true;

                        foreach (var identityError in createdIdentity.Errors)
                        {

                            var error = new Error { ErrorCode = ErrorCodes.IdentityCreationFailed, ErrorMessage = identityError.Description };
                            result.Errors.Add(error);
                        }
                        return result;
                    }
                    var profileInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName, request.UserName, request.PhoneNumber, request.DateOfBirth, request.CurrentCity);

                    var profile = UserProfile.CreateUserProfile(identity.Id, profileInfo);
                try
                {
                    _context.UserProfiles.Add(profile);

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtSettings.SigningKey);
                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, identity.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Email, identity.Email),
                        new Claim("IdentityId", identity.Id),
                        new Claim("UserProfileId", profile.UserProfileId.ToString())
                    }),
                    Expires = DateTime.Now.AddHours(2),
                    Audience = _jwtSettings.Audiences[0],
                    Issuer = _jwtSettings.Issuer,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                result.Payload =  tokenHandler.WriteToken(token);
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
