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

        protected BaseEnvelopeResponse()
        {
        }

        public TModel Result { get; protected set; }
        public bool IsSuccess => string.IsNullOrWhiteSpace(ErrorMessage);
        public string ErrorMessage { get; protected set; }
        public DateTime OccuredIn { get; protected set; }
        public HttpStatusCode Code { get; protected set; }
    }
}
