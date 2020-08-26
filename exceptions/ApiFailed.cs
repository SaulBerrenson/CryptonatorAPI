using System;

namespace CryptonatorApi.exceptions
{
    public class ApiFailed : Exception
    {
        public ApiFailed(string message) : base(message)
        {
        }

        public ApiFailed(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}