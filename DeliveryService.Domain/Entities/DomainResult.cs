using System.Linq;
using System.Net;

namespace DeliveryService.Domain.Entities
{
    public struct DomainResult
    {
        private static readonly DomainResult OkResult = new DomainResult(false, null, HttpStatusCode.OK);

        private readonly BaseDomainResult _baseDomainResult;

        public bool Fail => _baseDomainResult.Fail;
        public bool Success => _baseDomainResult.Success;
        public string ErrorMessage => _baseDomainResult.ErrorMessage;
        public HttpStatusCode Code => _baseDomainResult.Code;

        private DomainResult(bool failt, string errorMessage, HttpStatusCode code)
        {
            _baseDomainResult = new BaseDomainResult(failt, errorMessage, code);
        }

        public static DomainResult Ok()
        {
            return OkResult;
        }

        public static DomainResult RegisterFailure(string error, HttpStatusCode code = HttpStatusCode.BadRequest)
        {
            return new DomainResult(true, error, code);
        }

        public static DomainResult<T> Ok<T>(T value, HttpStatusCode code = HttpStatusCode.OK)
        {
            return new DomainResult<T>(false, value, null, code);
        }

        public static DomainResult<T> Failure<T>(string error, HttpStatusCode code = HttpStatusCode.BadRequest)
        {
            return new DomainResult<T>(true, default, error, code);
        }

        public static DomainResult CombineDomains(string errorMessagesSeparator, params DomainResult[] results)
        {
            var failedResults = results.Where(x => x.Fail).ToList();

            if (!failedResults.Any()) return Ok();

            var errorMessage = string.Join(errorMessagesSeparator, failedResults.Select(x => x.ErrorMessage).ToArray());

            return RegisterFailure(errorMessage);
        }

    }

    public class DomainResult<T>
    {
        private readonly BaseDomainResult _baseDomainResult;

        public DomainResult()
        {
            _baseDomainResult = new BaseDomainResult(false, "", HttpStatusCode.OK);
        }

        internal DomainResult(bool hasFail, T value, string errorMessage, HttpStatusCode code)
        {
            _baseDomainResult = new BaseDomainResult(hasFail, errorMessage, code);
            Value = value;
            Code = code;
        }

        public bool Success => _baseDomainResult.Success;
        public string ErrorMessage => _baseDomainResult.ErrorMessage;
        public T Value { get; private set; }
        public HttpStatusCode Code { get; set; }

        public static implicit operator DomainResult(DomainResult<T> resultado)
        {
            if (resultado.Success) return DomainResult.Ok();

            return DomainResult.RegisterFailure(resultado.ErrorMessage);
        }
    }
}
