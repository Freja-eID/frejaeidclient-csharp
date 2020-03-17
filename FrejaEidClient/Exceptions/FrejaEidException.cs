using System;

namespace Com.Verisec.FrejaEid.FrejaEidClient.Exceptions
{
    internal class FrejaEidException : Exception
    {
        public FrejaEidException(string message) : base(message) { }

        public FrejaEidException(string message, int errorCode) : base(message) {
            this.ErrorCode = errorCode;
        }

        public int ErrorCode { get; }

    }
}