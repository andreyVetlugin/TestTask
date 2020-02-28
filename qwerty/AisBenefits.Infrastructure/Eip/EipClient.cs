using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace AisBenefits.Infrastructure.Eip
{
    public static class EipClient
    {
        public static EipHttpRequestResult Request(EipHttpRequest eipHttpRequest, IEipLogger logger)
        {
            var request = (HttpWebRequest)WebRequest.Create(eipHttpRequest.Uri);

            request.KeepAlive = false;
            request.Method = eipHttpRequest.Verb;
            request.ContentType = "application/xml";
            request.SendChunked = false;
            request.Timeout = 600 * 1000;
            request.TransferEncoding = null;

            var bodyBytes = eipHttpRequest.Body != null ? Encoding.UTF8.GetBytes(eipHttpRequest.Body) : new byte[0];

            if (eipHttpRequest.Headers != null)
            {
                foreach (var header in eipHttpRequest.Headers.Where(t => t.Item1 != "Content-Length"))
                {
                    try
                    {
                        request.Headers[header.Item1] = header.Item2;
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }

            if (eipHttpRequest.Certificate != null)
            {
                request.Credentials = CredentialCache.DefaultCredentials;
                request.ClientCertificates.Add(eipHttpRequest.Certificate);
            }

            if (eipHttpRequest.SignMessage && eipHttpRequest.Certificate != null)
            {
                request.Headers["X-Signature"] = EipCryptography.Sign(bodyBytes, eipHttpRequest.Certificate);
            }

            logger.Info($"{eipHttpRequest.Verb}_{eipHttpRequest.Uri.AbsolutePath.Replace("/", "")}_request", PrepareRequestLog(request, eipHttpRequest.Body));

            if (request.Method != HttpVerb.GET)
            {
                request.ContentLength = bodyBytes.Length;

                var requestStream = request.GetRequestStream();
                var requestBuffer = bodyBytes;
                requestStream.Write(requestBuffer, 0, requestBuffer.Length);
                requestStream.Flush();
                requestStream.Close();
            }

            try
            {
                var response = (HttpWebResponse)request.GetResponse();

                if (response.ContentLength == 0)
                {
                    return new EipHttpRequestResult(null, response.StatusCode);
                }

                var responseBody = GetResponseBody(response);

                logger.Info($"{eipHttpRequest.Verb}_{eipHttpRequest.Uri.AbsolutePath.Replace("/", "")}_response", PrepareResponseLog(response, responseBody));

                return new EipHttpRequestResult(responseBody, response.StatusCode);

            }
            catch (WebException ex)
            {
                if (ex.Status != WebExceptionStatus.ProtocolError)
                {
                    throw;
                }

                var response = (HttpWebResponse)ex.Response;

                if (response.ContentLength == 0)
                {
                    return new EipHttpRequestResult(null, response.StatusCode);
                }

                var responseBody = GetResponseBody(response);

                logger.Error($"{eipHttpRequest.Verb}_{eipHttpRequest.Uri.AbsolutePath.Replace("/", "")}_response_err", PrepareResponseLog(response, responseBody));

                return new EipHttpRequestResult(responseBody, response.StatusCode);
            }
        }

        private static string GetResponseBody(HttpWebResponse response)
        {
            using (var responseStream = response.GetResponseStream())
            {
                string responseBody;

                using (var ms = new MemoryStream())
                {
                    responseStream.CopyTo(ms);

                    responseBody = Encoding.UTF8.GetString(ms.ToArray());
                }

                return responseBody;
            }
        }

        private static string PrepareResponseLog(HttpWebResponse response, string responseBody)
        {
            var result = string.Empty;

            result += string.Format("HTTP 1.1 {0} {1}", (int)response.StatusCode, response.StatusDescription);

            foreach (var h in response.Headers.AllKeys)
            {
                result += string.Format("{0}: {1}\r\n", h, response.Headers[h]);
            }

            result += "\r\n" + responseBody;

            return result;
        }

        private static string PrepareRequestLog(HttpWebRequest request, string requestBody)
        {
            var result = string.Empty;

            foreach (var h in request.Headers.AllKeys)
            {
                result += string.Format("{0}: {1}\r\n", h, request.Headers[h]);
            }

            result += "\r\n" + requestBody;

            return result;
        }
    }

    public interface IEipLogger
    {
        void Error(string context, string error);
        void Info(string context, string info);
    }

    public static class HttpVerb
    {
        public const string POST = "POST";
        public const string GET = "GET";
        public const string DELETE = "DELETE";
    }

    public static class EipCryptography
    {
        public static string Sign(byte[] message, X509Certificate2 certificate)
        {
            var unicode = Encoding.UTF8;

            var msgBytes = message.Length == 0 ? unicode.GetBytes("\r\n") : message;

            ContentInfo contentInfo = new ContentInfo(msgBytes);

            var signedCms = new SignedCms(contentInfo, true);

            var cmsSigner = new CmsSigner(certificate);
            
            signedCms.ComputeSignature(cmsSigner, false);

            return Convert.ToBase64String(signedCms.Encode());
        }
    }

    public class EipHttpRequest
    {
        public EipHttpRequest(Uri uri, string verb, bool signMessage, X509Certificate2 certificate, string body, Tuple<string, string>[] headers)
        {
            Uri = uri;
            Verb = verb;
            SignMessage = signMessage;
            Certificate = certificate;
            Body = body;
            Headers = headers;
        }

        public Uri Uri { get; private set; }
        public string Verb { get; private set; }

        public bool SignMessage { get; private set; }
        public X509Certificate2 Certificate { get; private set; }

        public string Body { get; private set; }

        public Tuple<string, string>[] Headers { get; private set; }
    }

    public struct EipHttpRequestResult
    {
        public EipHttpRequestResult(string body, HttpStatusCode code) : this()
        {
            Body = body;
            Code = code;
        }

        public string Body { get; private set; }
        public HttpStatusCode Code { get; private set; }
    }

    public static class EipHttpHeaders
    {
        public const string ASYNC = "X-Async";
    }
}