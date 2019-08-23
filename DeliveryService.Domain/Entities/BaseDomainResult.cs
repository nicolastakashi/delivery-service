using System;
using System.Net;

namespace DeliveryService.Domain.Entities
{
    internal sealed class BaseDomainResult
    {
        private readonly string _errorMessage;

        public BaseDomainResult(bool fail, string errorMessage, HttpStatusCode code)
        {
            if (fail)
            {
                if (string.IsNullOrEmpty(errorMessage))
                    throw new ArgumentNullException(nameof(ErrorMessage),
                        "There must be an Error Message for Error operations.");
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(errorMessage))
                    throw new ArgumentException("There should be no error message for successful operations.",
                        nameof(ErrorMessage));
            }

            Fail = fail;
            Code = code;
            _errorMessage = errorMessage;
        }

        public bool Fail { get; }
        public bool Success => !Fail;
        public HttpStatusCode Code;

        public string ErrorMessage
        {
            get
            {
                if (Success)
                    throw new InvalidOperationException("There should be no error message for successful operations.");

                return _errorMessage;
            }
        }
    }
}
