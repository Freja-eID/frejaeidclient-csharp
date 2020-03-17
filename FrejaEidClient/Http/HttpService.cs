using Com.Verisec.FrejaEid.FrejaEidClient.Beans.Common;
using Com.Verisec.FrejaEid.FrejaEidClient.Exceptions;
using Com.Verisec.FrejaEid.FrejaEidClient.Services;
using Com.Verisec.FrejaEid.FrejaEidClient.Constants;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Com.Verisec.FrejaEid.FrejaEidClient.Http
{
    public class HttpService : IHttpService
    {
        private static JsonService jsonService;

        private const int DEFAULT_NUMBER_OF_RETRIES = 3;
        private const int MAXIMUM_NUMBER_OF_HTTP_CONNECTIONS = 20;
        private const string POST_PARAMS_DELIMITER = "&";

        private const string VERSION_PLACEHOLDER = "%version%";
        private const string FREJA_EID_CLIENT_VERSION_INFO = "FrejaEidClient/" + VERSION_PLACEHOLDER;
        private const string DOT_NET_VERSION_INFO = "(.NET Framework/" + VERSION_PLACEHOLDER + ")";
        private const string CONTENT_TYPE = "application/json";
        private static string userAgentHeader;
        private readonly X509Certificate2 clientCertificate;
        private readonly X509Certificate2 serverCertificate;
        private readonly int connectionTimeout;
        private readonly int readTimeout;

        public HttpService(X509Certificate2 clientCertificate, X509Certificate2 serverCertificate, int connectionTimeout, int readTimeout)
        {
            jsonService = new JsonService();
            this.clientCertificate = clientCertificate;
            this.serverCertificate = serverCertificate;
            this.connectionTimeout = connectionTimeout;
            this.readTimeout = readTimeout;
            userAgentHeader = MakeUserAgentHeader();
        }
        public T Send<T>(Uri methodUrl, string requestTemplate, RelyingPartyRequest relyingPartyRequest, string relyingPartyId) where T : FrejaHttpResponse
        {
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            Enums.HttpStatusCode httpStatusCode;
            string responseString = String.Empty;
            int numberOfTries = 0;
            while (numberOfTries < DEFAULT_NUMBER_OF_RETRIES)
            {
                try
                {
                    request = (HttpWebRequest)HttpWebRequest.Create(methodUrl);
                    ServicePoint currentServicePoint = request.ServicePoint;
                    currentServicePoint.ConnectionLimit = MAXIMUM_NUMBER_OF_HTTP_CONNECTIONS;

                    string jsonRequest = jsonService.SerializeToJson(relyingPartyRequest);
                    string jsonRequestB64 = Base64Encode(jsonRequest);
                    string requestBody = String.Format(requestTemplate, jsonRequestB64);

                    if (relyingPartyId != null)
                    {
                        string relyingPartyIdRequest = String.Format(RequestTemplate.RELYING_PARTY_ID_TEMPLATE, relyingPartyId);
                        requestBody += POST_PARAMS_DELIMITER + relyingPartyIdRequest;
                    }

                    request.ClientCertificates = new X509Certificate2Collection { clientCertificate };
                    ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) =>
                    {
                        return errors == SslPolicyErrors.None && serverCertificate.GetCertHashString().Equals(certificate.GetCertHashString());
                    };
                    request.Method = HttpMethod.Post.ToString();

                    byte[] requestBytes = Encoding.UTF8.GetBytes(requestBody);

                    request.ContentLength = requestBytes.Length;
                    request.ContentType = CONTENT_TYPE;
                    request.UserAgent = userAgentHeader;

                    request.Timeout = connectionTimeout;
                    request.ReadWriteTimeout = readTimeout;

                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(requestBytes, 0, requestBytes.Length);
                    requestStream.Close();

                    response = (HttpWebResponse)request.GetResponse();

                    responseString = ExtractResponseFromStream(response);
                    return jsonService.DeserializeFromJson<T>(responseString);
                }
                catch (Exception e)
                {
                    numberOfTries++;
                    if (e is WebException webException)
                    {
                        Console.WriteLine(e.Message);
                        HttpWebResponse exceptionResponse = webException.Response as HttpWebResponse;
                        if (exceptionResponse != null)
                        {
                            httpStatusCode = (Enums.HttpStatusCode)((int)exceptionResponse.StatusCode);
                            switch (httpStatusCode)
                            {
                                case Enums.HttpStatusCode.NO_CONTENT:
                                    return (T)new FrejaHttpResponse();
                                case Enums.HttpStatusCode.BAD_REQUEST:
                                case Enums.HttpStatusCode.UNPROCESSABLE_ENTITY:
                                    FrejaHttpErrorResponse errorResponse = jsonService.DeserializeFromJson<FrejaHttpErrorResponse>(ExtractResponseFromStream(exceptionResponse));
                                    throw new FrejaEidException(errorResponse.Message, errorResponse.Code);
                                default:
                                    if (numberOfTries >= DEFAULT_NUMBER_OF_RETRIES)
                                    {
                                        throw new FrejaEidException(message: $"HTTP code {exceptionResponse.StatusCode}.");
                                    }
                                    break;
                            }
                        }
                    }
                    else
                    {
                        if (numberOfTries >= DEFAULT_NUMBER_OF_RETRIES)
                        {
                            throw new FrejaEidClientInternalException(message: "Failed to send HTTP request.", cause: e);
                        }
                    }
                }
                finally
                {
                    if (response != null)
                    {
                        try
                        {
                            response.Close();
                        }
                        catch (Exception ex)
                        {
                            throw new FrejaEidClientInternalException(message: "Failed to close HTTP connection.", cause: ex);
                        }
                    }
                }
            }
            throw new FrejaEidClientInternalException(message: "Failed to send HTTP request.");
        }

        protected static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        protected static string ExtractResponseFromStream(HttpWebResponse response)
        {
            if (response == null) { throw new FrejaEidClientInternalException(message: "Responce received from server is null"); }
            string jsonResponse = null;
            Encoding encode = Encoding.UTF8;
            using (Stream receiveStream = response.GetResponseStream())
            using (StreamReader readStream = new StreamReader(receiveStream, encode))
            {
                jsonResponse = readStream.ReadToEnd();
            }

            return jsonResponse;

        }

        internal static string MakeUserAgentHeader()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(FREJA_EID_CLIENT_VERSION_INFO.Replace(VERSION_PLACEHOLDER, GetLibVersion()));
            stringBuilder.Append(" ");
            stringBuilder.Append(DOT_NET_VERSION_INFO.Replace(VERSION_PLACEHOLDER, Environment.Version.ToString()));
            return stringBuilder.ToString();
        }

        internal static string GetLibVersion()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            return fvi.FileVersion;
        }
    }
}
