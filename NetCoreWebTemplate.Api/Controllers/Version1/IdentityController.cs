using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCoreWebTemplate.Api.Contracts.Version1.Requests;
using NetCoreWebTemplate.Api.Routes.Version1;
using NetCoreWebTemplate.Application.Identity.Commands.CreateAccount;
using System.Threading.Tasks;

namespace NetCoreWebTemplate.Api.Controllers.Version1
{
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public IdentityController(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        /// <summary>
        /// Creates an account for a new client
        /// </summary>
        /// <response code="201">Creates a New Client account</response>
        /// <response code="400">Unable to create new Client account due to validation error</response>
        /// <response code="429">Too Many Requests</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [HttpPost(ApiRoutes.Identity.Create)]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequest createAccountRequest)
        {
            var createAccountDto = mapper.Map<CreateAccountDto>(createAccountRequest);

            var command = new CreateAccountCommand(createAccountDto);
            var result = await mediator.Send(command);

            return CreatedAtAction("CreateAccount", result);
        }
    }
}
