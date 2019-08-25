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
    [Route("api/connections")]
    [ApiVersion("1")]
    public class ConnectionController : BaseController
    {
        private readonly IMediator _bus;
        private readonly IConnectionReadOnlyRepository _connectionReadOnlyRepository;

        public ConnectionController(IMediator bus, IConnectionReadOnlyRepository connectionReadOnlyRepository)
        {
            _bus = bus;
            _connectionReadOnlyRepository = connectionReadOnlyRepository;
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody]CreateConnectionCommand command)
        {
            var result = await _bus.Send(command);

            return result.Success
                ? Created(result.Value)
                : Error(result.ErrorMessage);
        }

        [HttpPut, Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromBody] UpdatedConnectionCommand command)
        {
            var result = await _bus.Send(command);

            return result.Fail
                ? Error(result.ErrorMessage)
                : NoContent();
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Inactive([FromRoute] string id)
        {
            var result = await _bus.Send(InactiveConnectionCommand.Create(id));

            return result.Fail
                ? Error(result.ErrorMessage)
                : NoContent();
        }

        [HttpGet("{id}"), Authorize]
        public async Task<IActionResult> Find([FromRoute]string id)
        {
            var connection = await _connectionReadOnlyRepository.FindAsync(id);

            return Success(connection);
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> Find([FromQuery]GetPagedResourceQuery resourceQuery)
        {
            var connections = await _connectionReadOnlyRepository.GetAsync(resourceQuery);

            return Success(connections);
        }
    }
}
