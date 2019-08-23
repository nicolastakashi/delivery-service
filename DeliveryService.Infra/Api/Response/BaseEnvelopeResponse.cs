using System;

namespace DeliveryService.Infra.Api.Response
{
    public class BaseEnvelopeResponse<TModel>
    {
        protected internal BaseEnvelopeResponse(TModel result, string errorMessage = "")
        {
            Result = result;
            ErrorMessage = errorMessage;
            OccuredIn = DateTime.UtcNow;
        }

        public TModel Result { get; }
        public bool IsSuccess => string.IsNullOrWhiteSpace(ErrorMessage);
        public string ErrorMessage { get; }
        public DateTime OccuredIn { get; }
    }
}
