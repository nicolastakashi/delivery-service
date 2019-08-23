using System;
using System.Net;

namespace DeliveryService.Domain.Entities
{
    internal sealed class BaseDomainResult
    {
        public BaseDomainResult(bool fail, string errorMessage, HttpStatusCode code)
        {
            Fail = fail;
            Code = code;
            ErrorMessage = errorMessage;
        }

        public bool Fail { get; }
        public bool Success => !Fail;
        public HttpStatusCode Code;
        public string ErrorMessage { get; private set; }
    }
}
