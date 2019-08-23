using System.Net;

namespace DeliveryService.Infra.Api.Response
{
    public class EnvelopeResponse : BaseEnvelopeResponse<string>
    {
        public EnvelopeResponse(string errorMessage = "", HttpStatusCode code = HttpStatusCode.OK)
            : base(string.Empty, errorMessage, code)
        {
        }

        public static BaseEnvelopeResponse<T> Success<T>(T result)
            => new BaseEnvelopeResponse<T>(result, "", HttpStatusCode.OK);

        public static EnvelopeResponse Success()
            => new EnvelopeResponse("", HttpStatusCode.OK);

        public static EnvelopeResponse Error(string errorMessage, HttpStatusCode code = HttpStatusCode.BadRequest)
            => new EnvelopeResponse(errorMessage, code);
    }
}
