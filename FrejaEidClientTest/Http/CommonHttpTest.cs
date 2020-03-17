using Com.Verisec.FrejaEid.FrejaEidClient.Beans.Common;
using Com.Verisec.FrejaEid.FrejaEidClient.Beans.General;
using Com.Verisec.FrejaEid.FrejaEidClient.Constants;
using Com.Verisec.FrejaEid.FrejaEidClient.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace Com.Verisec.FrejaEid.FrejaEidClient.Http.Tests
{
    public abstract class CommonHttpTest
    {
        protected static readonly string REFERENCE = "123456789012345678";
        protected static readonly string EMAIL = "eid.demo.verisec@gmail.com";
        protected static readonly string SSN = "123455697887";
        protected static readonly string RELYING_PARTY_ID = "relying_party_id";
        protected static readonly SsnUserInfo SSN_USER_INFO = SsnUserInfo.Create(Country.SWEDEN, SSN);
        protected static readonly BasicUserInfo BASIC_USER_INFO = new BasicUserInfo("John", "Fante");
        protected static readonly string CUSTOM_IDENTIFIER = "vejofan";
        protected static readonly string DETAILS = "Ask the dust";
        protected static readonly string RELYING_PARTY_USER_ID = "relyingPartyUserId";
        protected static readonly string DATE_OF_BIRTH = "1987-10-18";
        protected static readonly string EMAIL_ADDRESS = "test@frejaeid.com";
        protected static readonly string ORGANISATION_ID = "vealrad";
        protected static readonly RequestedAttributes REQUESTED_ATTRIBUTES = new RequestedAttributes(BASIC_USER_INFO, CUSTOM_IDENTIFIER, SSN_USER_INFO, null, DATE_OF_BIRTH, RELYING_PARTY_USER_ID, EMAIL_ADDRESS, ORGANISATION_ID);
        protected static readonly string POST_PARAMS_DELIMITER = "&";
        protected static readonly string KEY_VALUE_DELIMITER = "=";
        protected static readonly int MOCK_SERVICE_PORT = 30665;
        private HttpServer server;
        protected static JsonService jsonService;

        protected void StartMockServer<T>(T expectedRequest, int statusCodeToReturn, string responseToReturn) where T : RelyingPartyRequest
        {
            server = new HttpServer(MOCK_SERVICE_PORT, async context =>
            {
                HttpListenerRequest request = context.Request;
                Encoding encode = Encoding.UTF8;
                try
                {
                    using (StreamReader requestStream = new StreamReader(request.InputStream, encode))
                    {
                        string requestData = requestStream.ReadToEnd();
                        string[] postParams = requestData.Split(POST_PARAMS_DELIMITER.ToCharArray());
                        string RequestParam = postParams[0].Split(KEY_VALUE_DELIMITER.ToCharArray(), 2)[1];
                        if (postParams.Length == 2)
                        {
                            string relyingPartyIdParam = postParams[1].Split(KEY_VALUE_DELIMITER.ToCharArray(), 2)[1];
                            Assert.AreEqual(RELYING_PARTY_ID, relyingPartyIdParam);
                        }


                        string jsonReceivedRequest = Encoding.UTF8.GetString(Convert.FromBase64String(RequestParam));
                        string jsonExpectedRequest = jsonService.SerializeToJson(expectedRequest);
                        Assert.AreEqual(jsonExpectedRequest, jsonReceivedRequest);

                        T receivedRequest = jsonService.DeserializeFromJson<T>(jsonReceivedRequest);

                        Assert.AreEqual(expectedRequest, receivedRequest);

                    }
                }
                catch (Exception ex)
                {
                    Assert.Fail(ex.Message);
                }

                using (Stream responseStream = context.Response.OutputStream)
                {
                    context.Response.StatusCode = statusCodeToReturn;
                    context.Response.ContentLength64 = responseToReturn.Length;
                    await responseStream.WriteAsync(Encoding.UTF8.GetBytes(responseToReturn), 0, responseToReturn.Length).ConfigureAwait(false);
                }
            });
            server.Start();
        }


        public void StopServer()
        {
            if (server != null)
            {
                server.Stop();
            }
        }
    }

}