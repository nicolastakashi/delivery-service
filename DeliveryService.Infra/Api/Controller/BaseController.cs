using DeliveryService.Infra.Api.Response;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DeliveryService.Infra.Api.Controller
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected IActionResult Success<TResult>(TResult result)
            => Ok(EnvelopeResponse.Success(result));

        protected IActionResult Error(Exception exception) 
            => BadRequest(EnvelopeResponse.Error(exception.Message));

        protected IActionResult Success()
            => Ok(EnvelopeResponse.Success());
    }
}
