using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
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
    public class RegisterIdentityHandler : IRequestHandler<RegisterIdentity, OperationResult<string>>
    {
        private readonly DataContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IdentityService _identityService;

        public RegisterIdentityHandler(DataContext context, UserManager<IdentityUser> userManager, IdentityService identityService)
        {
            _context = context;
            _userManager = userManager;
            _identityService = identityService;
        }
        public async Task<OperationResult<string>> Handle(RegisterIdentity request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<string>();
            try
            {
                var creationValid = await IdentityDoesNotExistValidation(result, request);
                if (!creationValid) return result;


                await using var transaction = _context.Database.BeginTransaction();// so that If something fails inside this transaction to rollback and not do anything included in the using!
                var identity = await CreateIdentityUserAsync(result, request, transaction);
                if (identity == null) return result;
                var profile = await CreateUserProfileAsync(result, request, transaction, identity);
                await transaction.CommitAsync();



                result.Payload = GetJwtString(identity, profile);

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

        private async Task<bool> IdentityDoesNotExistValidation(OperationResult<string> result, RegisterIdentity request)
        {

            var existingIdentity = await _userManager.FindByEmailAsync(request.UserName);

            if (existingIdentity != null)
            {
                result.IsError = true;
                var error = new Error { ErrorCode = ErrorCodes.IdentityUserAlreadyExists, ErrorMessage = $"Email already exists.Can not Register" };
                result.Errors.Add(error);
                return false;
            }

            return true;
        }

        private async Task<IdentityUser> CreateIdentityUserAsync(OperationResult<string> result, RegisterIdentity request, IDbContextTransaction transaction)
        {
            var identity = new IdentityUser { Email = request.UserName, UserName = request.UserName };
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
                return null;
            }
            return identity;

        }

        private async Task<UserProfile> CreateUserProfileAsync(OperationResult<string> result, RegisterIdentity request, IDbContextTransaction transaction, IdentityUser identity)
        {
            try
            {
                var profileInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName, request.UserName, request.PhoneNumber, request.DateOfBirth, request.CurrentCity);

                var profile = UserProfile.CreateUserProfile(identity.Id, profileInfo);

                _context.UserProfiles.Add(profile);
                await _context.SaveChangesAsync();
                return profile;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private string GetJwtString(IdentityUser identity, UserProfile user)
        {
            var claimsIdentity = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, identity.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Email, identity.Email),
                        new Claim("IdentityId", identity.Id),
                        new Claim("UserProfileId", user.UserProfileId.ToString())
                    });
            var token = _identityService.CreateSecurityToken(claimsIdentity);

            return _identityService.PrintToken(token);
        }
    }
}
