using DeliveryService.Domain.Commands;
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
    [Route("api/routes")]
    public class RouteController : BaseController
    {
        private readonly IMediator _mediator;

        public RouteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost, Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(BaseEnvelopeResponse<ObjectId>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(EnvelopeResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(EnvelopeResponse), (int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> Create([FromBody]CreateRouteCommand command)
        {
            var result = await _mediator.Send(command);

            return result.Success
                ? Created(result.Value)
                : Error(result.ErrorMessage, result.Code);
        }

        [HttpPut, Authorize(Roles = "Admin")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(EnvelopeResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(EnvelopeResponse), (int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> Update([FromBody]UpdateRouteCommand command)
        {
            var result = await _mediator.Send(command);

            return result.Success
                ? NoContent()
                : Error(result.ErrorMessage, result.Code);
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(EnvelopeResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Inactive([FromRoute]string id)
        {
            var result = await _mediator.Send(InactiveRouteCommand.Create(id));

            return result.Success
                ? NoContent()
                : Error(result.ErrorMessage);
        }
    }
}
