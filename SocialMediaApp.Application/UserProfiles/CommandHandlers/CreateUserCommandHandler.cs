using MediatR;
using SocialMediaApp.Application.Enums;
using SocialMediaApp.Application.Models;
using SocialMediaApp.Application.UserProfiles.Commands;
using SocialMediaApp.Data;
using SocialMediaApp.Domain.Aggregates.UserProfileAggregate;
using SocialMediaApp.Domain.Exceptions;

namespace SocialMediaApp.Application.UserProfiles.CommandHandlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, OperationResult<UserProfile>>
    {
        private readonly DataContext _context;

        public CreateUserCommandHandler(DataContext context)
        {
            _context = context;
        }
        public async Task<OperationResult<UserProfile>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<UserProfile>();

            try
            {
                var basicInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName, request.EmailAdress, request.PhoneNumber,
                                                        request.DateOfBirth, request.CurrentCity);

                var userProfile = UserProfile.CreateUserProfile(Guid.NewGuid().ToString(), basicInfo);

                _context.UserProfiles.Add(userProfile);
                await _context.SaveChangesAsync();

                result.Payload = userProfile;

                return result;
            }catch(UserProfileNotValidException ex)
            {
                result.IsError = true;
                ex.ValidationErrors.ForEach(e =>
                {
                    var error = new Error { ErrorCode = ErrorCodes.ValidationError, ErrorMessage = $"{ex.Message}" };
                    result.Errors.Add(error);
                });

            }catch(Exception e)
            {
                var error = new Error { ErrorCode = ErrorCodes.UnknownError, ErrorMessage = $"{e.Message}" };
                result.IsError= true;
                result.Errors.Add(error);
            }

            return result;

        }
    }
}
