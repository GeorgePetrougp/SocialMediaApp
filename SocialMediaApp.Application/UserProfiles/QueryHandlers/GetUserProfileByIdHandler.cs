using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Application.Enums;
using SocialMediaApp.Application.Models;
using SocialMediaApp.Application.UserProfiles.Queries;
using SocialMediaApp.Data;
using SocialMediaApp.Domain.Aggregates.UserProfileAggregate;

namespace SocialMediaApp.Application.UserProfiles.QueryHandlers
{
    internal class GetUserProfileByIdHandler : IRequestHandler<GetUserProfileById, OperationResult<UserProfile>>
    {
        private readonly DataContext _context;

        public GetUserProfileByIdHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<UserProfile>> Handle(GetUserProfileById request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<UserProfile>();
            var profile =  await _context.UserProfiles.FirstOrDefaultAsync(up => up.UserProfileId == request.UserProfileId);

            if (profile is null)
            {
                result.AddError(ErrorCodes.NotFound, string.Format(UserProfileErrorMessages.UserProfileNotFound, request.UserProfileId));
                return result;
            }

            result.Payload = profile;

            return result;
        }
    }
}
