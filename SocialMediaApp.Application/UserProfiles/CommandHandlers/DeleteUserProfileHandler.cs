using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Application.Enums;
using SocialMediaApp.Application.Models;
using SocialMediaApp.Application.UserProfiles.Commands;
using SocialMediaApp.Data;
using SocialMediaApp.Domain.Aggregates.UserProfileAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Application.UserProfiles.CommandHandlers
{
    internal class DeleteUserProfileHandler : IRequestHandler<DeleteUserProfile, OperationResult<UserProfile>>
    {
        private readonly DataContext _context;

        public DeleteUserProfileHandler(DataContext context)
        {
            _context = context;
        }
        public async Task<OperationResult<UserProfile>> Handle(DeleteUserProfile request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<UserProfile>();

            var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(up => up.UserProfileId == request.UserProfileId);

            if (userProfile is null)
            {
                result.AddError(ErrorCodes.NotFound, string.Format(UserProfileErrorMessages.UserProfileNotFound, request.UserProfileId));
               
                return result;
            }

            _context.UserProfiles.Remove(userProfile);
            await _context.SaveChangesAsync(cancellationToken);

            result.Payload = userProfile;

            return result;

        }
    }
}
