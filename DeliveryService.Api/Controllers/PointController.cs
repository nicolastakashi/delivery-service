using DeliveryService.Domain.Commands;
using DeliveryService.Domain.Queries;
using DeliveryService.Domain.Repositories.Readonly;
using DeliveryService.Infra.Api.Controller;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Create([FromBody]CreatePointCommand command)
        {
            var result = await _bus.Send(command);

            return result.Success
                ? Created()
                : Error(result.ErrorMessage);
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Remove([FromRoute]string id)
        {
            var command = InactivePointCommand.Create(id);

            var result = await _bus.Send(command);

            return result.Success
                ? NoContent()
                : Error(result.ErrorMessage);
        }

        [HttpPut, Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromBody]UpdatePointCommand command)
        {
            var result = await _bus.Send(command);

            return result.Success
                ? NoContent()
                : Error(result.ErrorMessage);
        }

        [HttpGet("{id}"), Authorize]
        public async Task<IActionResult> Find([FromRoute]string id)
        {
            var point = await _pointReadOnlyRepository.Find(id);

            return Success(point);
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> Find([FromQuery]GetPagedResourceQuery resourceQuery)
        {
            var point = await _pointReadOnlyRepository.Get(resourceQuery);

            return Success(point);
        }
    }
}
