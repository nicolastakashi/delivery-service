using DeliveryService.Domain.Commands;
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

        public ConnectionController(IMediator bus)
        {
            _bus = bus;
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

        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> Inactive([FromRoute] string id)
        {
            var result = await _bus.Send(InactiveConnectionCommand.Create(id));

            return result.Fail
                ? Error(result.ErrorMessage)
                : NoContent();
        }
    }
}
