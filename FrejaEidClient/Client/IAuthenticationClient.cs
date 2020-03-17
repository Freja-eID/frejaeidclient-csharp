using Com.Verisec.FrejaEid.FrejaEidClient.Beans.Authentication;
using System.Collections.Generic;

namespace Com.Verisec.FrejaEid.FrejaEidClient.Client
{
    public interface IAuthenticationClient
    {
        string Initiate(InitiateAuthenticationRequest initiateAuthenticationRequest);

        AuthenticationResult GetResult(AuthenticationResultRequest authenticationResultRequest);

        List<AuthenticationResult> GetResults(AuthenticationResultsRequest authenticationResultsRequest);

        AuthenticationResult PollForResult(AuthenticationResultRequest authenticationResultRequest, int maxWaitingTimeInSec);

        void Cancel(CancelAuthenticationRequest cancelAuthenticationRequest);
    }
}
