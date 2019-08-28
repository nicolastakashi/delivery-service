using System;
using System.Runtime.Serialization;

namespace DeliveryService.Infra.CustomExceptions
{
    public class UserFriendlyException : Exception
    {
        public UserFriendlyException() : base()
        {
        }

        public UserFriendlyException(string message) : base(message)
        {
        }

        public UserFriendlyException(string message, Exception inner) : base(message, inner)
        {
        }

        protected UserFriendlyException(SerializationInfo info, StreamingContext context)
        {
        }
    }
}
