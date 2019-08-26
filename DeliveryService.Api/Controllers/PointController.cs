using DeliveryService.Domain.Commands;
using DeliveryService.Domain.Queries;
using DeliveryService.Domain.Queries.Result;
using DeliveryService.Domain.Repositories.Readonly;
using DeliveryService.Infra.Api.Controller;
using DeliveryService.Infra.Api.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Net;
using System.Threading.Tasks;

namespace DeliveryService.Api.Controllers
{
    [Route("api/points")]
    [ApiVersion("1")]
    public class PointController : BaseController
    {
        private readonly IMediator _bus;
        private readonly IPointReadOnlyRepository _pointReadOnlyRepository;

        public PointController(IMediator bus, IPointReadOnlyRepository pointReadOnlyRepository)
        {
            _bus = bus;
            _pointReadOnlyRepository = pointReadOnlyRepository;
        }

        [HttpPost, Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(BaseEnvelopeResponse<ObjectId>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(EnvelopeResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(EnvelopeResponse), (int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> Create([FromBody]CreatePointCommand command)
        {
            var result = await _bus.Send(command);

            return result.Success
                ? Created(result.Value)
                : Error(result.ErrorMessage);
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(EnvelopeResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Remove([FromRoute]string id)
        {
            var command = InactivePointCommand.Create(id);

            var result = await _bus.Send(command);

            return result.Success
                ? NoContent()
                : Error(result.ErrorMessage);
        }

        [HttpPut, Authorize(Roles = "Admin")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(EnvelopeResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(EnvelopeResponse), (int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> Update([FromBody]UpdatePointCommand command)
        {
            var result = await _bus.Send(command);

            return result.Success
                ? NoContent()
                : Error(result.ErrorMessage);
        }

        [HttpGet("{id}"), Authorize]
        [ProducesResponseType(typeof(BaseEnvelopeResponse<PointQueryResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(EnvelopeResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Find([FromRoute]string id)
        {
            var point = await _pointReadOnlyRepository.FindAsync(id);

            return Success(point);
        }

        [HttpGet, Authorize]
        [ProducesResponseType(typeof(BaseEnvelopeResponse<PagedQueryResult<PointQueryResult>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(EnvelopeResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Find([FromQuery]GetPagedResourceQuery resourceQuery)
        {
            var points = await _pointReadOnlyRepository.GetAsync(resourceQuery);

            return Success(points);
        }
    }
}
