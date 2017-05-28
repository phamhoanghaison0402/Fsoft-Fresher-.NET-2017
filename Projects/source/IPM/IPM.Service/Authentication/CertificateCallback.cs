using System.Net;

namespace Authentication
{
    /// <summary>
    /// CertificateCallback
    /// </summary>
    public static class CertificateCallback
  {
        /// <summary>
        /// Initializes the <see cref="CertificateCallback"/> class.
        /// </summary>
        static CertificateCallback()
    {
      ServicePointManager.ServerCertificateValidationCallback = CertificateValidationCallBack;
    }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public static void Initialize()
    {
    }

        /// <summary>
        /// Certificates the validation call back.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="certificate">The certificate.</param>
        /// <param name="chain">The chain.</param>
        /// <param name="sslPolicyErrors">The SSL policy errors.</param>
        /// <returns></returns>
     private static bool CertificateValidationCallBack(
         object sender,
         System.Security.Cryptography.X509Certificates.X509Certificate certificate,
         System.Security.Cryptography.X509Certificates.X509Chain chain,
         System.Net.Security.SslPolicyErrors sslPolicyErrors)
    {
      // If the certificate is a valid, signed certificate, return true.
      if (sslPolicyErrors == System.Net.Security.SslPolicyErrors.None)
      {
        return true;
      }
      // If there are errors in the certificate chain, look at each error to determine the cause.
      if ((sslPolicyErrors & System.Net.Security.SslPolicyErrors.RemoteCertificateChainErrors) != 0)
      {
        if (chain != null && chain.ChainStatus != null)
        {
          foreach (System.Security.Cryptography.X509Certificates.X509ChainStatus status in chain.ChainStatus)
          {
            if ((certificate.Subject == certificate.Issuer) &&
               (status.Status == System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.UntrustedRoot))
            {
              // Self-signed certificates with an untrusted root are valid. 
              continue;
            }
            else
            {
              if (status.Status != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError)
              {
                // If there are any other errors in the certificate chain, the certificate is invalid,
                // so the method returns false.
                return false;
              }
            }
          }
        }
        // When processing reaches this line, the only errors in the certificate chain are 
        // untrusted root errors for self-signed certificates. These certificates are valid
        // for default Exchange server installations, so return true.
        return true;
      }
      else
      {
        // In all other cases, return false.
        return false;
      }
    }
  }
}
