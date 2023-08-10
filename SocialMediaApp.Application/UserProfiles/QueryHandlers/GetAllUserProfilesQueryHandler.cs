using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Application.Models;
using SocialMediaApp.Application.UserProfiles.Queries;
using SocialMediaApp.Data;
using SocialMediaApp.Domain.Aggregates.UserProfileAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Application.UserProfiles.QueryHandlers
{
    public class GetAllUserProfilesQueryHandler : IRequestHandler<GetAllUserProfiles, OperationResult<IEnumerable<UserProfile>>>
    {
        private readonly DataContext _context;

        public GetAllUserProfilesQueryHandler(DataContext context)
        {
            _context = context;
        }
        public async Task<OperationResult<IEnumerable<UserProfile>>> Handle(GetAllUserProfiles request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<IEnumerable<UserProfile>>();

            result.Payload = await _context.UserProfiles.ToListAsync();

            return result;
        }
    }
}
