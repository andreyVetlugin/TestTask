using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;

namespace AisBenefits.Infrastructure.Eip
{
    public static class SertificateProvider
    {
        public static X509Certificate2 GetSerCertificate(string certificateThumbprint)
        {
            var x509Store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            x509Store.Open(OpenFlags.ReadOnly);
            var certCollection = x509Store.Certificates.Find(X509FindType.FindByThumbprint, certificateThumbprint, false);

            if (certCollection.Count == 0)
                return null;

            return certCollection[0];

        }
    }
}
