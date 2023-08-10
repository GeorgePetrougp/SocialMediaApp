using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Application.Enums;
using SocialMediaApp.Application.Models;
using SocialMediaApp.Application.UserProfiles.Commands;
using SocialMediaApp.Data;
using SocialMediaApp.Domain.Aggregates.UserProfileAggregate;
using SocialMediaApp.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Application.UserProfiles.CommandHandlers
{
    internal class UpdateUserProfileBasicInfoHandler : IRequestHandler<UpdateUserProfileBasicInfo, OperationResult<UserProfile>>
    {
        private readonly DataContext _context;

        public UpdateUserProfileBasicInfoHandler(DataContext context)
        {
            _context = context;
        }
        public async Task<OperationResult<UserProfile>> Handle(UpdateUserProfileBasicInfo request, CancellationToken cancellationToken)
        {

            var result = new OperationResult<UserProfile>();
            try
            {
                var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(up => up.UserProfileId == request.UserProfileId);

                if(userProfile is null)
                {
                    result.IsError = true;
                    var error = new Error {ErrorCode = ErrorCodes.NotFound, ErrorMessage = $"No User Profile with ID{request.UserProfileId} found" };
                    result.Errors.Add(error);
                    return result;
                }

                var basicInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName, request.EmailAdress, request.PhoneNumber,
                                                            request.DateOfBirth, request.CurrentCity);

                userProfile.UpdateBasicInfo(basicInfo);

                _context.UserProfiles.Update(userProfile);
                await _context.SaveChangesAsync();

                result.Payload = userProfile;
                return result;

            }
            catch (UserProfileNotValidException ex)
            {
                result.IsError = true;
                ex.ValidationErrors.ForEach(e =>
                {
                    var error = new Error { ErrorCode = ErrorCodes.ValidationError, ErrorMessage = $"{ex.Message}" };
                    result.Errors.Add(error);
                });

                return result;
            }


            catch (Exception ex)
            {
                var error = new Error { ErrorCode = ErrorCodes.ServerError, ErrorMessage = ex.Message };
                result.IsError = true;
                result.Errors.Add(error);

            }
            
            return result;
        }
    }
}
