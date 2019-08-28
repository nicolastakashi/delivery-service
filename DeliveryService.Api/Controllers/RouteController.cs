using DeliveryService.Domain.Commands;
using DeliveryService.Domain.Queries;
using DeliveryService.Domain.Queries.Result;
using DeliveryService.Domain.Repositories.Readonly;
using DeliveryService.Domain.ValueObject;
using DeliveryService.Infra.Api.Controller;
using DeliveryService.Infra.Api.Response;
using DeliveryService.Infra.Data.Constants;
using DeliveryService.Infra.Data.Context;
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
        private readonly IRouteReadOnlyRepository _routeReadOnlyRepository;
        private readonly IRedisContext _redisContext;

        public RouteController(IMediator mediator, IRouteReadOnlyRepository routeReadOnlyRepository, IRedisContext redisContext)
        {
            _mediator = mediator;
            _routeReadOnlyRepository = routeReadOnlyRepository;
            _redisContext = redisContext;
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

        [HttpGet, Authorize]
        [ProducesResponseType(typeof(BaseEnvelopeResponse<PagedQueryResult<RoutesQueryResult>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(EnvelopeResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Get([FromQuery]GetPagedResourceQuery resource)
        {
            var routes = await _routeReadOnlyRepository.GetAsync(resource);

            return Success(routes);
        }

        [HttpGet("{id}"), Authorize]
        [ProducesResponseType(typeof(BaseEnvelopeResponse<PagedQueryResult<RoutesQueryResult>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(EnvelopeResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Find([FromRoute]string id)
        {
            var cacheKey = $"{CacheKeys.Routes}:{id}";

            var routePath = _redisContext.Get<BestRoutePath>(cacheKey);

            if (routePath is null)
            {
                var result = await _mediator.Send(new FindTheBestRoutePathCommand(id));

                if (result.Success is false) return Error(result.ErrorMessage);

                routePath = result.Value;

                _routeReadOnlyRepository.SaveOnCache(cacheKey, routePath);
            }

            return Success(routePath);
        }
    }
}
