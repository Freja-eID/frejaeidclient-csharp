using Com.Verisec.FrejaEid.FrejaEidClient.Beans.Authentication;
using Com.Verisec.FrejaEid.FrejaEidClient.Beans.Common;
using Com.Verisec.FrejaEid.FrejaEidClient.Enums;
using Com.Verisec.FrejaEid.FrejaEidClient.Exceptions;
using Com.Verisec.FrejaEid.FrejaEidClient.Http;
using Com.Verisec.FrejaEid.FrejaEidClient.Constants;
using System;
using System.Threading;

namespace Com.Verisec.FrejaEid.FrejaEidClient.Services
{
    public class AuthenticationService : BasicService
    {
        private readonly int pollingTimeoutInMilliseconds;
        private readonly TransactionContext transactionContext;

        public AuthenticationService(string serverAddress, IHttpService httpService, int pollingTimeoutInMilliseconds, TransactionContext transactionContext) : base(serverAddress, httpService)
        {
            this.pollingTimeoutInMilliseconds = pollingTimeoutInMilliseconds;
            this.transactionContext = transactionContext;
        }

        public AuthenticationService(string serverAddress, IHttpService httpService) : base(serverAddress, httpService)
        {
            this.pollingTimeoutInMilliseconds = 0;
            this.transactionContext = TransactionContext.PERSONAL;
        }

        public InitiateAuthenticationResponse Initiate(InitiateAuthenticationRequest initiateAuthenticationRequest)
        {
            string methodUri = transactionContext == TransactionContext.ORGANISATIONAL ? MethodUrl.ORGANISATION_AUTHENTICATION_INIT : MethodUrl.AUTHENTICATION_INIT;
            return httpService.Send<InitiateAuthenticationResponse>(GetUri(serverAddress, methodUri), RequestTemplate.INIT_AUTHENTICATION_TEMPLATE, initiateAuthenticationRequest, initiateAuthenticationRequest.RelyingPartyId);
        }

        public AuthenticationResult GetResult(AuthenticationResultRequest authenticationResultRequest)
        {
            string methodUri = transactionContext == TransactionContext.ORGANISATIONAL ? MethodUrl.ORGANISATION_AUTHENTICATION_GET_ONE_RESULT : MethodUrl.AUTHENTICATION_GET_RESULT;
            return httpService.Send<AuthenticationResult>(GetUri(serverAddress, methodUri), RequestTemplate.AUTHENTICATION_RESULT_TEMPLATE, authenticationResultRequest, authenticationResultRequest.RelyingPartyId);
        }

        public AuthenticationResult PollForResult(AuthenticationResultRequest authenticationResultRequest, int maxWaitingTimeInSec)
        {
            long pollingEndTime = Convert.ToInt64(DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond + TimeSpan.FromSeconds(maxWaitingTimeInSec).TotalMilliseconds);
            while (maxWaitingTimeInSec == 0 || ((DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond + pollingTimeoutInMilliseconds) < pollingEndTime))
            {
                AuthenticationResult authenticationResult = GetResult(authenticationResultRequest);
                if (maxWaitingTimeInSec == 0 || IsFinalStatus(authenticationResult.Status))
                {
                    return authenticationResult;
                }
                try
                {
                    Thread.Sleep(pollingTimeoutInMilliseconds);
                }
                catch (Exception ex)
                {
                    throw new FrejaEidClientInternalException(message: $"An error occured while waiting to make another request with {maxWaitingTimeInSec} polling timeout.", cause: ex);
                }
            }
            throw new FrejaEidClientPollingException(message: $"A timeout of {maxWaitingTimeInSec} was reached while sending request.");
        }

        public AuthenticationResults GetResults(AuthenticationResultsRequest authenticationResultsRequest)
        {
            string methodUri = transactionContext == TransactionContext.ORGANISATIONAL ? MethodUrl.ORGANISATION_AUTHENTICATION_GET_RESULTS : MethodUrl.AUTHENTICATION_GET_RESULTS;
            return httpService.Send<AuthenticationResults>(GetUri(serverAddress, methodUri), RequestTemplate.AUTHENTICATION_RESULTS_TEMPLATE, authenticationResultsRequest, authenticationResultsRequest.RelyingPartyId);
        }

        public EmptyFrejaResponse Cancel(CancelAuthenticationRequest cancelAuthenticationRequest)
        {
            string methodUri = transactionContext == TransactionContext.ORGANISATIONAL ? MethodUrl.ORGANISATION_AUTHENTICATION_CANCEL : MethodUrl.AUTHENTICATION_CANCEL;
            return httpService.Send<EmptyFrejaResponse>(GetUri(serverAddress, methodUri), RequestTemplate.CANCEL_AUTHENTICATION_TEMPLATE, cancelAuthenticationRequest, cancelAuthenticationRequest.RelyingPartyId);
        }

        public TransactionContext GetTransactionContext()
        {
            return transactionContext;
        }
    }
}