using System;
using System.Linq;
using System.Net;
using Microsoft.EntityFrameworkCore.Internal;

namespace AisBenefits.Infrastructure.Eip.Pfr.PfrBapForPeriodSmev3
{
    public static class PfrBapForPeriodClient
    {
        public static PfrBapForPeriodClientRequestInitial RequestInitial(IPfrBapForPeriodClientRequestTarget target,
            IPfrBapForPeriodClientConfig config, IEipLogger logger)
        {
            var request = new BapForPeriodRequestType
            {
                FrguCode = config.FrguCode,
                NumberOfMonths = "1",
                Period = target.Date.ToString("yyyy-MM"),
                Snils = target.Snils,
                ResidentInfo = new ResidentInfoType
                {
                    BirthDate = target.BirthDate.Date,
                    Gender = TransformTargetGender(target.Gender),
                    FIO = new FIOType
                    {
                        MiddleName = target.MiddleName,
                        FirstName = target.FirstName,
                        LastName = target.SurName
                    }
                }
            };

            var result = EipClient.Request(
                new EipHttpRequest(
                    new Uri(config.Uri + "/AsyncRequest"),
                    HttpVerb.POST,
                    config.SignMessage,
                    config.CertificateThumbprint != null
                        ? SertificateProvider.GetSerCertificate(config.CertificateThumbprint)
                        : null,
                    EipXmlTextSerializer.Serialize(request),
                    new[]
                    {
                        Tuple.Create(EipHttpHeaders.ASYNC, target.RequestId.ToString())
                    }
                ),
                logger
            );

            return new PfrBapForPeriodClientRequestInitial(result.Code == HttpStatusCode.Accepted);
        }

        static GenderType TransformTargetGender(string gender)
        {
            switch (gender)
            {
                case "м":
                case "М":
                    return GenderType.Male;
                case "ж":
                case "Ж":
                    return GenderType.Female;
                default:
                    throw new InvalidOperationException();
            }
        }

        public static PfrBapForPeriodClientRequestResult RequestResult(IPfrBapForPeriodClientRequestTarget target,
            IPfrBapForPeriodClientConfig config, IEipLogger logger)
        {
            var result = EipClient.Request(
                new EipHttpRequest(
                    new Uri(config.Uri + $"/AsyncRequest/{target.RequestId}"),
                    HttpVerb.GET,
                    config.SignMessage,
                    config.CertificateThumbprint != null
                        ? SertificateProvider.GetSerCertificate(config.CertificateThumbprint)
                        : null,
                    null,
                    null
                ),
                logger
            );

            if (result.Code == HttpStatusCode.OK)
            {
                var response = EipXmlTextSerializer.Deserialize<BapForPeriodResponseType>(result.Body);

                var gospension = response.MonthlyPayment?.FirstOrDefault()?.Payment
                                     .Select(p => (priority: MapGosPensionType(p.Payment.Type), amount: p.Payment.Sum))
                                     .Where(p => p.priority > 0)
                                     .OrderBy(p => p.priority)
                                     .Select(p => p.amount)
                                     .FirstOrDefault() ?? 0;

                return new PfrBapForPeriodClientRequestResult(true, true, gospension);
            }

            if (result.Code == HttpStatusCode.Accepted)
            {
                return new PfrBapForPeriodClientRequestResult(true, false, 0);
            }

            return new PfrBapForPeriodClientRequestResult(false, false, 0);
        }

        static int MapGosPensionType(string type)
        {
            if (type.Contains("3.47") ||
                type.Contains("3.37") ||
                type.Contains("3.19") ||
                type.Contains("2.47") ||
                type.Contains("2.37") ||
                type.Contains("2.19"))
                return 1;

            if (type.Contains("5.0"))
                return 2;

            return 0;
        }

        public static void DeleteRequest(IPfrBapForPeriodClientRequestTarget target,
            IPfrBapForPeriodClientConfig config, IEipLogger logger)
        {
            EipClient.Request(
                new EipHttpRequest(
                    new Uri(config.Uri + $"/AsyncRequest/{target.RequestId}"),
                    HttpVerb.DELETE,
                    config.SignMessage,
                    config.CertificateThumbprint != null
                        ? SertificateProvider.GetSerCertificate(config.CertificateThumbprint)
                        : null,
                    null,
                    null
                ),
                logger
            );
        }

        public static void Clear(IPfrBapForPeriodClientConfig config, IEipLogger logger)
        {
            var result = EipClient.Request(
                new EipHttpRequest(
                    new Uri(config.Uri + $"/AsyncRequest"),
                    HttpVerb.GET,
                    config.SignMessage,
                    config.CertificateThumbprint != null
                        ? SertificateProvider.GetSerCertificate(config.CertificateThumbprint)
                        : null,
                    null,
                    null
                ),
                logger
            );

            if (result.Code == HttpStatusCode.OK)
            {
                var states = EipXmlTextSerializer.Deserialize<AsyncResultsType>(result.Body);
                foreach (var state in states.AsyncResult)
                    DeleteRequest(new DeleteTarget(Guid.Parse(state.RequestId)), config, logger);
            }
        }

        class DeleteTarget : IPfrBapForPeriodClientRequestTarget
        {
            public DeleteTarget(Guid requestId)
            {
                RequestId = requestId;
            }

            public string MiddleName => throw new NotImplementedException();
            public string FirstName => throw new NotImplementedException();
            public string SurName => throw new NotImplementedException();
            public string Snils => throw new NotImplementedException();
            public DateTime BirthDate => throw new NotImplementedException();
            public string Gender => throw new NotImplementedException();
            public DateTime Date => throw new NotImplementedException();
            public Guid RequestId { get; }
        }
    }


    public interface IPfrBapForPeriodClientRequestTarget
    {
        string MiddleName { get; }
        string FirstName { get; }
        string SurName { get; }

        string Snils { get; }
        DateTime BirthDate { get; }
        string Gender { get; }

        DateTime Date { get; }
        Guid RequestId { get; }
    }

    public interface IPfrBapForPeriodClientConfig
    {
        string Uri { get; }
        string FrguCode { get; }

        bool SignMessage { get; }
        string CertificateThumbprint { get; }

        int Timeout { get; }
    }

    public class PfrBapForPeriodClientRequestInitial
    {
        public bool Ok { get; }

        public PfrBapForPeriodClientRequestInitial(bool ok)
        {
            Ok = ok;
        }
    }

    public class PfrBapForPeriodClientRequestResult
    {
        public bool Ok { get; }

        public bool Done { get; }

        public decimal GosPensionAmount { get; }

        public PfrBapForPeriodClientRequestResult(bool ok, bool done, decimal gosPensionAmount)
        {
            Ok = ok;
            Done = done;
            GosPensionAmount = gosPensionAmount;
        }
    }
}
