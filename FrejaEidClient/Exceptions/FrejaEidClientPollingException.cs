using System;

namespace Com.Verisec.FrejaEid.FrejaEidClient.Exceptions
{
    internal class FrejaEidClientPollingException : Exception
    {
        public FrejaEidClientPollingException(String message) : base(message)
        {
        }
    }
}