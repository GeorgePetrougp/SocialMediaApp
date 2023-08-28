using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Application.Enums;
using SocialMediaApp.Application.Identity.Commands;
using SocialMediaApp.Application.Models;
using SocialMediaApp.Application.UserProfiles;
using SocialMediaApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Application.Identity.CommandHandlers
{
    public class RemoveAccountHandler : IRequestHandler<RemoveAccount, OperationResult<bool>>
    {
        private readonly DataContext _context;

        public RemoveAccountHandler(DataContext context)
        {
            _context = context;
            
        }

        public async Task<OperationResult<bool>> Handle(RemoveAccount request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<bool>();

            try
            {
                var identityUser = await _context.Users.FirstOrDefaultAsync(i => i.Id == request.IdentityUserId.ToString());

                if(identityUser == null)
                {
                    result.AddError(ErrorCodes.IdentityUserDoesNotExist, IdentityErrorMessages.NonExistantIdentityUser);
                    return result;
                }

                var userProfile = await _context.UserProfiles
                                .Where(u => u.IdentityId == request.IdentityUserId.ToString())
                                .FirstOrDefaultAsync(cancellationToken);

                if(userProfile == null)
                {
                    result.AddError(ErrorCodes.NotFound, UserProfileErrorMessages.UserProfileNotFound);
                    return result;
                }

                if(identityUser.Id != request.RequesterGuid.ToString())
                {
                    result.AddError(ErrorCodes.UnauthorizedAccountRemoval, IdentityErrorMessages.UnauthorisedAccountRemoval);
                    return result;
                }

                _context.UserProfiles.Remove(userProfile);
                _context.Users.Remove(identityUser);
                await _context.SaveChangesAsync(cancellationToken);

                result.Payload = true;
            }
            catch (Exception ex)
            {
                result.AddUnknownError(ex.Message);
            }

            return result;
        }
    }
}
