using System;
using System.Collections.Generic;
using System.Text;

namespace YoulaApi.Exceptions
{
    public class YoulaException : Exception
    {
        public int Code { get; }

        public YoulaException() : base() { }

        public YoulaException(string message) : base(message) { }

        public YoulaException(int code, string message) : base(message)
        {
            Code = code;
        }


    }
}
