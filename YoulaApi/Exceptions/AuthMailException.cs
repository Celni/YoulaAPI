using System;
using System.Collections.Generic;
using System.Text;

namespace YoulaApi.Exceptions
{
    public class AuthMailException : Exception
    {
        public AuthMailException() : base() { }

        public AuthMailException(string message) : base(message) { }
    }
}
