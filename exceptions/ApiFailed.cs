using System;

namespace CryptonatorAPI.exceptions
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