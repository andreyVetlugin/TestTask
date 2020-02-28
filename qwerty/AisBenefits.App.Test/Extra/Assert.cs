using System.IO;
using Microsoft.AspNetCore.Http;

using NAssert = NUnit.Framework.Assert;

namespace AisBenefits.App.Test.Extra
{
    static class Assert
    {
        public static void StatusCode(int statusCode, HttpResponse response)
        {
            response.Body.Position = 0;
            NAssert.AreEqual(statusCode, response.StatusCode, new StreamReader(response.Body).ReadToEnd());
        }
    }
}
