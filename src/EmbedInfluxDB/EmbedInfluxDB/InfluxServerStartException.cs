using System;
using System.Runtime.Serialization;

namespace EmbedInfluxDB
{
    [Serializable]
    internal class InfluxServerStartException : Exception
    {
        public InfluxServerStartException()
        {
        }

        public InfluxServerStartException(string message) : base(message)
        {
        }

        public InfluxServerStartException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InfluxServerStartException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}