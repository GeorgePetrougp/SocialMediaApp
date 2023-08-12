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
using SocialMediaApp.Domain.Exceptions;
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
                await IdentityDoesNotExistValidation(result, request);
                if (result.IsError) return result;


                await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);// so that If something fails inside this transaction to rollback and not do anything included in the using!

                var identity = await CreateIdentityUserAsync(result, request, transaction,cancellationToken);
                if (result.IsError) return result;

                var profile = await CreateUserProfileAsync(result, request, transaction, identity,cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                result.Payload = GetJwtString(identity, profile);
                return result;

            }
            catch(UserProfileNotValidException ex)
            {
                ex.ValidationErrors.ForEach(error => {result.AddError(ErrorCodes.ValidationError, error);});
            }
            catch (Exception ex)
            {
                result.AddUnknownError(ex.Message);
               
            }
            return result;
        }

        private async Task IdentityDoesNotExistValidation(OperationResult<string> result, RegisterIdentity request)
        {

            var existingIdentity = await _userManager.FindByEmailAsync(request.UserName);

            if (existingIdentity != null) result.AddError(ErrorCodes.IdentityUserAlreadyExists, IdentityErrorMessages.EmailAlreadyExists);

        }
        private async Task<IdentityUser> CreateIdentityUserAsync(OperationResult<string> result, RegisterIdentity request, IDbContextTransaction transaction, CancellationToken cancellationToken)
        {
            var identity = new IdentityUser { Email = request.UserName, UserName = request.UserName };
            var createdIdentity = await _userManager.CreateAsync(identity, request.Password);
            if (!createdIdentity.Succeeded)
            {
                await transaction.RollbackAsync(cancellationToken);


                foreach (var identityError in createdIdentity.Errors)
                {
                    result.AddError(ErrorCodes.IdentityCreationFailed, identityError.Description);                 
                }
            }
            return identity;

        }

        private async Task<UserProfile> CreateUserProfileAsync(OperationResult<string> result, RegisterIdentity request, IDbContextTransaction transaction, IdentityUser identity, CancellationToken cancellationToken)
        {
            try
            {
                var profileInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName, request.UserName, request.PhoneNumber, request.DateOfBirth, request.CurrentCity);

                var profile = UserProfile.CreateUserProfile(identity.Id, profileInfo);

                _context.UserProfiles.Add(profile);
                await _context.SaveChangesAsync(cancellationToken);
                return profile;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
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
