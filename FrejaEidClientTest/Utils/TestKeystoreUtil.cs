using System.IO;

namespace Com.Verisec.FrejaEid.FrejaEidClientTest.Utils
{
    class TestKeystoreUtil
    {
        public const string CERTIFICATE_PATH = "certificates/validServerCertificate.cer";
        public const string KEYSTORE_PASSWORD = "123123123";
        public const string INVALID_KEYSTORE_FILE = "keystores/invalidKeystoreFile.txt";
        public const string KEYSTORE_PATH_PKCS12 = "keystores/test.p12";

        public static string GetKeystorePath(string FileName)
        {
            return Path.GetFullPath(@"../../../Resources/" + FileName);
        }

    }
}
