using Com.Verisec.FrejaEid.FrejaEidClient.Beans.Common;
using System;

namespace Com.Verisec.FrejaEid.FrejaEidClient.Http
{
    public interface IHttpService
    {
         T Send<T>(Uri methodUrl, string requestTemplate, RelyingPartyRequest relyingPartyRequest, string relyingPartyId) where T : FrejaHttpResponse;
    }
}
