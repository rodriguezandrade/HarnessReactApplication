using System;
using System.Net;

namespace Viq.AccessPoint.TestHarness.Services.Infrastructure.Exceptions
{
    public class CustomException : Exception
    {
        public CustomException(string message, Exception ex) 
            : base(message, ex)
        {

        }
    }
}
