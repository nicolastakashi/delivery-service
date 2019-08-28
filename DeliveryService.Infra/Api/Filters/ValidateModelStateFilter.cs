using DeliveryService.Infra.Api.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace DeliveryService.Infra.Api.Filters
{
    public class ValidateModelStateFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid is false)
            {
                var modelState = context.ModelState;
                var erros = modelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(erro => erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message);

                context.Result = new BadRequestObjectResult(EnvelopeResponse.Error(string.Join(", ", erros).Replace(",", Environment.NewLine)));
            }
        }
    }
}
