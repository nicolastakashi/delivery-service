using DeliveryService.Domain.Commands;
using DeliveryService.Domain.Queries;
using DeliveryService.Domain.Queries.Result;
using DeliveryService.Domain.Repositories.Readonly;
using DeliveryService.Infra.Api.Controller;
using DeliveryService.Infra.Api.Response;
using DeliveryService.Infra.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Net;
using System.Threading.Tasks;

namespace DeliveryService.Api.Controllers
{
    [Route("api/connections")]
    [ApiVersion("1")]
    public class ConnectionController : BaseController
    {
        private readonly IMediator _bus;
        private readonly IConnectionReadOnlyRepository _connectionReadOnlyRepository;

        public ConnectionController(IMediator bus, IConnectionReadOnlyRepository connectionReadOnlyRepository, IRedisContext redisContext)
        {
            _bus = bus;
            _connectionReadOnlyRepository = connectionReadOnlyRepository;
        }

        [HttpPost, Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(BaseEnvelopeResponse<ObjectId>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(EnvelopeResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(EnvelopeResponse), (int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> Create([FromBody]CreateConnectionCommand command)
        {
            var result = await _bus.Send(command);

            return result.Success
                ? Created(result.Value)
                : Error(result.ErrorMessage, result.Code);
        }

        [HttpPut, Authorize(Roles = "Admin")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(EnvelopeResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(EnvelopeResponse), (int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> Update([FromBody] UpdatedConnectionCommand command)
        {
            var result = await _bus.Send(command);

            return result.Fail
                ? Error(result.ErrorMessage, result.Code)
                : NoContent();
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(EnvelopeResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Inactive([FromRoute] string id)
        {
            var result = await _bus.Send(InactiveConnectionCommand.Create(id));

            return result.Fail
                ? Error(result.ErrorMessage, result.Code)
                : NoContent();
        }

        [HttpGet("{id}"), Authorize]
        [ProducesResponseType(typeof(BaseEnvelopeResponse<ConnectionQueryResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(EnvelopeResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Find([FromRoute]string id)
        {
            var connection = await _connectionReadOnlyRepository.FindAsync(id);

            return Success(connection);
        }

        [HttpGet, Authorize]
        [ProducesResponseType(typeof(BaseEnvelopeResponse<PagedQueryResult<ConnectionQueryResult>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(EnvelopeResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Find([FromQuery]GetPagedResourceQuery resource)
        {
            var connections = await _connectionReadOnlyRepository.GetAsync(resource);

            return Success(connections);
        }
    }
}
