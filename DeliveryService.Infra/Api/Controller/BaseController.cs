using DeliveryService.Infra.Api.Response;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DeliveryService.Infra.Api.Controller
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected IActionResult Success<TResult>(TResult result)
            => Ok(EnvelopeResponse.Success(result));

        protected IActionResult Error(string message, HttpStatusCode code = HttpStatusCode.BadRequest)
        {
            var result = new ObjectResult(EnvelopeResponse.Error(message, code))
            {
                StatusCode = (int)code
            };

            return result;
        }

        protected IActionResult Created()
            => new ObjectResult(EnvelopeResponse.Success())
            {
                StatusCode = (int)HttpStatusCode.Created
            };

        protected IActionResult Success()
            => Ok(EnvelopeResponse.Success());
    }
}
