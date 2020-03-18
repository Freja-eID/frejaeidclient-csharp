using Com.Verisec.FrejaEid.FrejaEidClient.Enums;
using Com.Verisec.FrejaEid.FrejaEidClient.Exceptions;
using Com.Verisec.FrejaEid.FrejaEidClient.Http;
using Com.Verisec.FrejaEid.FrejaEidClient.Services;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("FrejaEidClientTest")]

namespace Com.Verisec.FrejaEid.FrejaEidClient.Client
{
    public class BasicClient
    {
        private const int DEFAULT_CONNECTION_TIMEOUT_IN_MILLISECONDS = 20000;
        private const int DEFAULT_READ_TIMEOUT_IN_MILLISECONDS = 20000;
        private const int DEFAULT_POLLING_TIMEOUT_IN_MILLISECONDS = 3000;
        private const int MINIMUM_POLLING_TIMEOUT_IN_MILLISECONDS = 1000;
        private const int MAXIMUM_POLLING_TIMEOUT_IN_MILLISECONDS = 30000;
        protected JsonService jsonService;
        protected AuthenticationService authenticationService;

        protected BasicClient(string serverCutomUrl, int pollingTimeoutInMilliseconds, TransactionContext transactionContext, IHttpService httpService)
        {
            jsonService = new JsonService();
            authenticationService = new AuthenticationService(serverCutomUrl, httpService, pollingTimeoutInMilliseconds, transactionContext);
        }

        public abstract class GenericBuilder
        {

            protected string serverCustomUrl = null;
            protected int connectionTimeout = DEFAULT_CONNECTION_TIMEOUT_IN_MILLISECONDS;
            protected int readTimeout = DEFAULT_READ_TIMEOUT_IN_MILLISECONDS;
            protected int pollingTimeout = DEFAULT_POLLING_TIMEOUT_IN_MILLISECONDS;
            protected IHttpService httpService;
            protected readonly X509Certificate2 clientCertificate;
            protected readonly X509Certificate2 serverCertificate;
            protected TransactionContext transactionContext = TransactionContext.PERSONAL;

            public GenericBuilder(string keystorePath, string keystorePass, string certificatePath, string frejaEnvironment)
            {
                try
                {
                    clientCertificate = CreateClientCertificate(keystorePath, keystorePass);
                }
                catch (Exception e)
                {
                    throw new FrejaEidClientInternalException(message: "Failed to initialize SSL Context with given client certificate path or password.", cause: e);
                }


                if (certificatePath != null)
                {
                    try
                    {
                        serverCertificate = CreateServerCertificate(certificatePath, keystorePass);
                    }
                    catch (Exception e)
                    {
                        throw new FrejaEidClientInternalException(message: "Failed to initialize SSL Context with given server certificate path or password.", cause: e);
                    }

                }
                serverCustomUrl = frejaEnvironment;

            }

            private X509Certificate2 CreateClientCertificate(string keystorePath, string keystorePass)
            {
                byte[] keystoreBytes = System.IO.File.ReadAllBytes(keystorePath);
                return new X509Certificate2(keystoreBytes, keystorePass);
            }

            private X509Certificate2 CreateServerCertificate(string certificatePath, string keystorePass)
            {
                byte[] serverKeystoreBytes = System.IO.File.ReadAllBytes(certificatePath);
                return new X509Certificate2(serverKeystoreBytes, keystorePass);
            }

            public GenericBuilder SetConnectionTimeout(int connectionTimeout)
            {
                this.connectionTimeout = connectionTimeout;
                return this;
            }

            public GenericBuilder SetReadTimeout(int readTimeout)
            {
                this.readTimeout = readTimeout;
                return this;
            }

            public GenericBuilder SetPollingTimeout(int pollingTimeout)
            {
                this.pollingTimeout = pollingTimeout;
                return this;
            }

            internal GenericBuilder SetHttpService(IHttpService httpService)
            {
                this.httpService = httpService;
                return this;
            }

            public GenericBuilder SetTestModeCustomUrl(string serverCustomUrl)
            {
                this.serverCustomUrl = serverCustomUrl;
                return this;
            }

            public GenericBuilder SetTransactionContext(TransactionContext transactionContext)
            {
                this.transactionContext = transactionContext;
                return this;
            }

            public abstract T Build<T>() where T : BasicClient;

            protected void CheckSetParameters()
            {
                if (pollingTimeout < MINIMUM_POLLING_TIMEOUT_IN_MILLISECONDS || pollingTimeout > MAXIMUM_POLLING_TIMEOUT_IN_MILLISECONDS)
                {
                    throw new FrejaEidClientInternalException(message: $"Polling timeout must be between {MINIMUM_POLLING_TIMEOUT_IN_MILLISECONDS / 1000} and {MAXIMUM_POLLING_TIMEOUT_IN_MILLISECONDS / 1000} seconds.");
                }
            }

        }
    }

}

