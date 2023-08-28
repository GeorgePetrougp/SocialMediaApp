using SocialMediaApp.Api.Contracts.Identity;
using SocialMediaApp.Api.Filters;
using SocialMediaApp.Application.Identity.Commands;

namespace SocialMediaApp.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route(ApiRoutes.BaseRoute)]
    [ApiController]
    public class IdentityController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public IdentityController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        [Route(ApiRoutes.Identity.Registration)]
        [ValidateModel]
        public async Task<IActionResult> Register(UserRegistration registration)
        {
            var command = _mapper.Map<RegisterIdentity>(registration);
            var result = await _mediator.Send(command);


            if (result.IsError) return HandleErrorResponse(result.Errors);

            var authenticationResult = new AuthenticationResult() { Token = result.Payload };

            return Ok(authenticationResult);
        }

        [HttpPost]
        [Route(ApiRoutes.Identity.Login)]
        [ValidateModel]
        public async Task<IActionResult> Login(UserLogIn login)
        {
            var command = _mapper.Map<LoginCommand>(login);
            var result = await _mediator.Send(command);

            if (result.IsError) return HandleErrorResponse(result.Errors);

            var authenticationResult = new AuthenticationResult() { Token = result.Payload };

            return Ok(authenticationResult);
        }

        [HttpDelete]
        [Route(ApiRoutes.Identity.IdentityById)]
        [ValidateGuid("identityUserId")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteAccount(string identityUserId, CancellationToken cancellationToken)
        {
            var identityUserGuid = Guid.Parse(identityUserId);
            var requesterGuid = HttpContext.GetIdentityIdClaimValue();

            var command = new RemoveAccount{IdentityUserId = identityUserGuid, RequesterGuid = requesterGuid  };

            var result = await _mediator.Send(command);

            if (result.IsError) return HandleErrorResponse(result.Errors);

            return NoContent();

        }
    }
}
