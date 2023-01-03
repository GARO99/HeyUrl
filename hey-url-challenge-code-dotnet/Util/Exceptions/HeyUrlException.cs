using System;
using System.Runtime.Serialization;

namespace hey_url_challenge_code_dotnet.Util.Exceptions
{
    [Serializable]
    public class HeyUrlException : Exception
    {
        public HeyUrlException()
        {}

        public HeyUrlException(string message) : base(message)
        {}

        public HeyUrlException(string message, Exception innerException) : base(message, innerException)
        {}

        protected HeyUrlException(SerializationInfo info, StreamingContext context) : base(info, context)
        {}
    }
}
