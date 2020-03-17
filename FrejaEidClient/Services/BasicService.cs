using Com.Verisec.FrejaEid.FrejaEidClient.Enums;
using Com.Verisec.FrejaEid.FrejaEidClient.Http;
using Com.Verisec.FrejaEid.FrejaEidClient.Constants;
using System.Security.Policy;
using System;
using Com.Verisec.FrejaEid.FrejaEidClient.Exceptions;

namespace Com.Verisec.FrejaEid.FrejaEidClient.Services
{
    public class BasicService
    {
        protected IHttpService httpService;
        protected string serverAddress;

        public BasicService(string serverAddress, IHttpService httpService)
        {
            this.httpService = httpService;
            this.serverAddress = serverAddress;
        }

        protected Uri GetUri(string serverAddress, string Uri)
        {
            try
            {
                return new Uri(serverAddress + Uri);
            }
            catch (UriFormatException ex)
            {
                throw new FrejaEidClientInternalException(message: $"Invalid Url {serverAddress + Uri}", cause: ex);
            }

        }

        protected bool IsFinalStatus(TransactionStatus status)
        {
            return (status == TransactionStatus.CANCELED || status == TransactionStatus.RP_CANCELED || status == TransactionStatus.EXPIRED || status == TransactionStatus.APPROVED || status == TransactionStatus.REJECTED);
        }
    }
}