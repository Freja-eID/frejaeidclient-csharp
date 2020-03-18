using Com.Verisec.FrejaEid.FrejaEidClient.Beans.Authentication;
using Com.Verisec.FrejaEid.FrejaEidClient.Beans.General;
using Com.Verisec.FrejaEid.FrejaEidClient.Enums;
using Com.Verisec.FrejaEid.FrejaEidClient.Exceptions;
using Com.Verisec.FrejaEid.FrejaEidClient.Http;
using Com.Verisec.FrejaEid.FrejaEidClient.Services;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("FrejaEidClientTest")]

namespace Com.Verisec.FrejaEid.FrejaEidClient.Client
{
    public class AuthenticationClient : BasicClient, IAuthenticationClient
    {

        public AuthenticationClient(string serverCustomUrl, int pollingTimeoutInMillseconds, TransactionContext transactionContext, IHttpService httpService) : base(serverCustomUrl, pollingTimeoutInMillseconds, transactionContext, httpService) { }

        public static Builder Create(SslSettings sslSettings, string frejaEnvironment)
        {
            if (sslSettings == null) throw new FrejaEidClientInternalException(message: "SslSettings cannot be null.");
            return new Builder(sslSettings.KeystorePath, sslSettings.KeystorePass, sslSettings.ServerCertificatePath, frejaEnvironment);
        }
        public string Initiate(InitiateAuthenticationRequest initiateAuthenticationRequest)
        {
            RequestValidationService.ValidateInitAuthRequest(initiateAuthenticationRequest, authenticationService.GetTransactionContext());
            return authenticationService.Initiate(initiateAuthenticationRequest).AuthRef;
        }
        public AuthenticationResult GetResult(AuthenticationResultRequest authenticationResultRequest)
        {
            RequestValidationService.ValidateResultRequest(authenticationResultRequest);
            return authenticationService.GetResult(authenticationResultRequest);
        }

        public AuthenticationResult PollForResult(AuthenticationResultRequest authenticationResultRequest, int maxWaitingTimeInSec)
        {
            RequestValidationService.ValidateResultRequest(authenticationResultRequest);
            return authenticationService.PollForResult(authenticationResultRequest, maxWaitingTimeInSec);
        }

        public List<AuthenticationResult> GetResults(AuthenticationResultsRequest authenticationResultsRequest)
        {
            RequestValidationService.ValidateResultsRequest(authenticationResultsRequest);
            return authenticationService.GetResults(authenticationResultsRequest).AllAuthenticationResults;

        }
        public void Cancel(CancelAuthenticationRequest cancelAuthenticationRequest)
        {
            RequestValidationService.ValidateCancelRequest(cancelAuthenticationRequest);
            authenticationService.Cancel(cancelAuthenticationRequest);
        }

        public class Builder : GenericBuilder
        {
            public Builder(string keystorePath, string keystorePass, string certificatePath, string frejaEnvironment) : base(keystorePath, keystorePass, certificatePath, frejaEnvironment) { }

            public override AuthenticationClient Build<AuthenticationClient>()
            {
                CheckSetParameters();
                if (httpService == null)
                {
                    httpService = new HttpService(clientCertificate, serverCertificate, connectionTimeout, readTimeout);
                }
                return System.Activator.CreateInstance(typeof(AuthenticationClient), serverCustomUrl, pollingTimeout, transactionContext, httpService) as AuthenticationClient;
            }
        }

    }
}
