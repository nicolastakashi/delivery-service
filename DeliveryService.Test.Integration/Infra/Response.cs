using System;
using System.Net;

namespace DeliveryService.Test.Integration.Infra
{
    public class Response<TModel>
    {
        public TModel Result { get; set; }
        public bool IsSuccess => string.IsNullOrWhiteSpace(ErrorMessage);
        public string ErrorMessage { get; set; }
        public DateTime OccuredIn { get; set; }
        public HttpStatusCode Code { get; set; }
    }
}
