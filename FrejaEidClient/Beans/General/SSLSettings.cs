using Com.Verisec.FrejaEid.FrejaEidClient.Exceptions;
using System;

namespace Com.Verisec.FrejaEid.FrejaEidClient.Beans.General
{
    public class SslSettings
    {
        private SslSettings(string keystorePath, string keystorePass, string serverCertificatePath)
        {
            this.KeystorePath = keystorePath;
            this.KeystorePass = keystorePass;
            this.ServerCertificatePath = serverCertificatePath;
        }

        public static SslSettings Create(string keystorePath, string keystorePass, string serverCertificatePath)
        {
            if (String.IsNullOrWhiteSpace(keystorePath) || String.IsNullOrWhiteSpace(keystorePass) || String.IsNullOrWhiteSpace(serverCertificatePath))
            {
                throw new FrejaEidClientInternalException("KeyStore Path, keyStore password or server certificate path cannot be null or empty.");
            }
            return new SslSettings(keystorePath, keystorePass, serverCertificatePath);
        }

        public static SslSettings Create(string keystorePath, string keystorePass)
        {
            if (String.IsNullOrWhiteSpace(keystorePath) || String.IsNullOrWhiteSpace(keystorePass))
            {
                throw new FrejaEidClientInternalException("KeyStore Path or keyStore password cannot be null or empty.");
            }
            return new SslSettings(keystorePath, keystorePass, null);
        }

        public string KeystorePath { get; }
        public string KeystorePass { get; }
        public string ServerCertificatePath { get; }


    }


}