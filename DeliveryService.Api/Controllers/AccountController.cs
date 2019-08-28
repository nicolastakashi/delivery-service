using DeliveryService.Domain.Commands;
using DeliveryService.Infra.Api.Controller;
using DeliveryService.Infra.Api.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace DeliveryService.Api.Controllers
{
    [Route("api/accounts")]
    [ApiVersion("1")]
    public class AccountController : BaseController
    {
        private readonly IMediator _bus;

        public AccountController(IMediator bus)
        {
            _bus = bus;
        }

        [HttpPost, AllowAnonymous]
        [ProducesResponseType(typeof(BaseEnvelopeResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(EnvelopeResponse), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Login([FromBody]CreateUserSessionCommand command)
        {
            var result = await _bus.Send(command);

            return result.Success
                ? Success(result.Value)
                : Error(result.ErrorMessage, result.Code);
        }
    }
}
