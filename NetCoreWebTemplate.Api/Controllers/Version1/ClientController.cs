using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCoreWebTemplate.Api.Contracts.Version1.Requests;
using NetCoreWebTemplate.Api.Routes.Version1;
using NetCoreWebTemplate.Application.Clients.Commands.CreateClient;
using NetCoreWebTemplate.Application.Clients.Queries.GetClientDetails;
using System.Threading.Tasks;

namespace NetCoreWebTemplate.Api.Controllers.Version1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public ClientController(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        /// <summary>
        /// Creates a New Client
        /// </summary>
        /// <response code="201">Creates a New Client</response>
        /// <response code="400">Unable to create Client due to validation error</response>
        /// <response code="429">Too Many Requests</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [HttpPost(ApiRoutes.Client.Create)]
        public async Task<IActionResult> Create([FromBody] ClientRequest clientRequest)
        {
            var clientViewModel = mapper.Map<ClientViewModel>(clientRequest);

            var command = new CreateClientCommand(clientViewModel);
            var result = await mediator.Send(command);

            return CreatedAtAction("CreateClient", result);
        }

        /// <summary>
        /// Returns a Client specified by an Id
        /// </summary>
        /// <response code="200">Returns a Client specified by an Id</response>
        /// <response code="400">Unable to return Client due to invalid Id</response>
        /// <response code="429">Too Many Requests</response>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [HttpPost(ApiRoutes.Client.Get)]
        public async Task<IActionResult> Get(long clientId)
        {
            var query = new GetClientByIdQuery(clientId);
            var result = await mediator.Send(query);

            return Ok(result);
        }
    }
}