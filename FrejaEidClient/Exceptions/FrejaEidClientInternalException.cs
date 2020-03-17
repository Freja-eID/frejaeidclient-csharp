using System;

namespace Com.Verisec.FrejaEid.FrejaEidClient.Exceptions
{
    internal class FrejaEidClientInternalException : Exception
    {
        public FrejaEidClientInternalException(string message) : base(message) { }

        public FrejaEidClientInternalException(string message, Exception cause) : base(message, cause) { }

    }
}
