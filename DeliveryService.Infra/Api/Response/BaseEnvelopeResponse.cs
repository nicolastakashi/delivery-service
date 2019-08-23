using System;
using System.Net;

namespace DeliveryService.Infra.Api.Response
{
    public class BaseEnvelopeResponse<TModel>
    {
        protected internal BaseEnvelopeResponse(TModel result, string errorMessage = "", HttpStatusCode code = HttpStatusCode.OK)
        {
            Result = result;
            ErrorMessage = errorMessage;
            OccuredIn = DateTime.UtcNow;
            Code = code;
        }

        public TModel Result { get; }
        public bool IsSuccess => string.IsNullOrWhiteSpace(ErrorMessage);
        public string ErrorMessage { get; }
        public DateTime OccuredIn { get; }
        public HttpStatusCode Code { get; set; }
    }
}
