using AisBenefits.Infrastructure.Services.DropDowns;
using System;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace AisBenefits.Infrastructure.Eip.Pfr.Snils
{
    public class PfrSnilsClient
    {
        private readonly Uri uri;
        private readonly bool signMessage;
        private readonly X509Certificate2 certificate;
        private readonly string frguCode;
        private readonly IEipLogger logger;

        public PfrSnilsClient(Uri uri, bool signMessage, X509Certificate2 certificate, string frguCode, IEipLogger logger)
        {
            this.uri = uri;
            this.signMessage = signMessage;
            this.certificate = certificate;
            this.frguCode = frguCode;
            this.logger = logger;
        }

        public bool RequestInitial<TTarget>(TTarget target)
            where TTarget : IPfrSnilsRequestTarget
        {
            var result = EipClient.Request(
                new EipHttpRequest(new Uri(uri + "/AsyncRequest"), HttpVerb.POST, signMessage, certificate,
                    EipXmlTextSerializer.Serialize(
                        new SnilsAddDataRequestType
                        {
                            FrguCode = frguCode,
                            FLInfo = new FLInfoType
                            {
                                FIO = new FIOType
                                {
                                    FirstName = target.FirstName,
                                    LastName = target.SurName,
                                    MiddleName = target.MiddleName
                                },
                                BirthDate = target.BirthDate,
                                Gender = TransformTargetGender(target.Gender)
                            },
                            AdditionalData = new AdditionalDataType
                            {
                                IdentityDocument = GetDocument(target)
                            }
                        }
                        ),
                                    new[]
                    {
                        Tuple.Create(EipHttpHeaders.ASYNC, target.Id.ToString())
                    }),
                    logger
                );

            return result.Code == HttpStatusCode.Accepted;

            static DocumentType GetDocument(TTarget target)
            {
                switch (target.DocType)
                {
                    case DocumentTypes.PASSPORT:
                    default:
                        return new DocumentType
                        {
                            ItemElementName = ItemChoiceType.PassportRF,
                            Item = new PassportType
                            {
                                Series = target.DocSeria,
                                Number = target.DocNumber,
                                IssueDate = target.DocIssueDate,
                                Issuer = PfrSnilsClient.TransformDocIssuePlace(target.DocIssuePlace)
                            }
                        };
                }
            }
        }

        public bool RequestResult<TTarget>(TTarget target)
            where TTarget : IPfrSnilsRequestTarget
        {
            var result = EipClient.Request(
                new EipHttpRequest(
                    new Uri(uri + $"/AsyncRequest/{target.Id}"),
                    HttpVerb.GET,
                    signMessage,
                    certificate,
                    null,
                    null
                ),
                logger
            );

            switch (result.Code)
            {
                case HttpStatusCode.OK:
                    try
                    {
                        var response = EipXmlTextSerializer.Deserialize<SnilsAddDataResponseType>(result.Body);

                        target.SetSnils(response.Snils);

                        EipClient.Request(
                            new EipHttpRequest(
                                new Uri(uri + $"/AsyncRequest/{target.Id}"),
                                HttpVerb.DELETE,
                                signMessage,
                                certificate,
                                null,
                                null
                            ),
                            logger
                        );

                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                case HttpStatusCode.Accepted:

                    return true;

                default:
                    target.SetError();
                    return false;
            }
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

        static string TransformDocIssuePlace(string issuePlace)
        {
            if (string.IsNullOrWhiteSpace(issuePlace))
                return "-";
            issuePlace = string.Join(" ", Regex.Matches(issuePlace, $@"[а-яА-Я0-9\s]+").Cast<Match>().Select(m => m.Value));

            if (string.IsNullOrWhiteSpace(issuePlace))
                return "-";
            return issuePlace;
        }
    }

    public interface IPfrSnilsRequestTarget
    {
        Guid Id { get; }

        string FirstName { get; }
        string SurName { get; }
        string MiddleName { get; }
        DateTime BirthDate { get; }

        string Gender { get; }

        string DocType { get; }
        string DocSeria { get; }
        string DocNumber { get; }
        DateTime DocIssueDate { get; }
        string DocIssuePlace { get; }

        void SetSended();
        void SetError();

        void SetSnils(string snils);
    }

    public enum PfrSnilsQueryState3 : byte
    {
        AwaitingRequest = 0,
        AwaitingResponse = 1,
        NotFound = 2,
        FoundSingle = 3,
        Error = 5,
        HandledResult = 6
    }
}
