using AisBenefits.Infrastructure.Services.DropDowns;
using DataLayer.Entities.EGISSO;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AisBenefits.Infrastructure.Services.Egisso
{
    public static class EgissoEjectionService
    {
        public static void WriteEgissoFactZipXml<TContext>(this TContext context, Stream outStream, IEnumerable<IEgissoFact> facts)
            where TContext : IEgissoEjectionContext
        {
            string providerCode = context.ProviderCode;
            string oszCode = context.OszCode;
            
            using (var archive = new ZipArchive(outStream, ZipArchiveMode.Create, true))
            {
                int index = 0;
                foreach (var factPart in facts.Select((f, i) => (f, i)).GroupBy(t => t.i / 5000).Select(g => g.Select(t => t.f)))
                {
                    var packageId = Guid.NewGuid();
                    var xmlBuilder = new XmlBuilder(oszCode, packageId);

                    xmlBuilder.Reset(packageId);

                    foreach (var fact in factPart)
                        xmlBuilder.AddFact(fact);

                    using (var entry = archive.CreateEntry($"{oszCode.Split('.')[0]}_{providerCode}_{(++index):D3}.xml").Open())
                    using (var streamWriter = new StreamWriter(entry, Encoding.UTF8))
                        streamWriter.Write(xmlBuilder.Bake());
                }
            }
        }

        class XmlBuilder
        {
            private readonly StringBuilder sbResult = new StringBuilder();

            private readonly EjectionTransform transform = new EjectionTransform();

            private readonly string oszCode;

            public XmlBuilder(string oszCode, Guid packageId)
            {
                this.oszCode = oszCode;

                Reset(packageId);
            }

            public void Reset(Guid packageId)
            {
                sbResult.Clear();

                sbResult.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
                sbResult.Append("<ns14:data xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"urn://x-artefacts-smev-gov-ru/supplementary/commons/1.0.1\" xmlns:ns2=\"urn://egisso-ru/msg/10.12.I/1.0.0\" xmlns:ns3=\"urn://egisso-ru/types/prsn-request/1.0.4\" xmlns:ns4=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:ns5=\"urn://egisso-ru/types/package-RAF/1.0.5\" xmlns:ns6=\"urn://egisso-ru/types/assignment-fact/1.0.5\" xmlns:ns7=\"urn://egisso-ru/types/prsn-info/1.0.3\" xmlns:ns8=\"urn://egisso-ru/types/basic/1.0.6\" xmlns:ns9=\"urn://egisso-ru/msg/10.06.S/1.0.4\" xmlns:ns10=\"urn://egisso-ru/types/package-RAF/1.0.5\" xmlns:ns11=\"urn://egisso-ru/types/assignment-fact/1.0.5\" xmlns:ns12=\"urn://egisso-ru/types/prsn-info/1.0.3\" xmlns:ns13=\"urn://egisso-ru/types/basic/1.0.4\" xmlns:ns14=\"urn://egisso-ru/msg/10.06.S/1.0.4\" xmlns:ns15=\"urn://egisso-ru/types/package-protocol/1.0.4\" xmlns:ns16=\"urn://egisso-ru/types/package-result/1.0.4\" xmlns:ns17=\"urn://egisso-ru/types/record-result/1.0.4\" xmlns:ns18=\"urn://egisso-ru/msg/10.07.I/1.0.6\" xmlns:ns19=\"urn://egisso-ru/types/record-result/1.0.4\" xmlns:ns20=\"urn://egisso-ru/types/package-protocol/1.0.4\" xmlns:ns21=\"urn://egisso-ru/types/package-RO/1.0.5\" xmlns:ns22=\"urn://egisso-ru/types/organization/1.0.5\" xmlns:ns23=\"urn://egisso-ru/msg/10.07.I/1.0.6\" xmlns:ns24=\"urn://egisso-ru/types/package-RO/1.0.5\" xmlns:ns25=\"urn://egisso-ru/types/organization/1.0.5\" xmlns:ns26=\"urn://egisso-ru/msg/10.05.I/1.0.5\" xmlns:ns27=\"urn://egisso-ru/types/package-LMSZ/1.0.5\" xmlns:ns28=\"urn://egisso-ru/types/local-MSZ/1.0.4\" xmlns:ns29=\"urn://egisso-ru/msg/10.05.I/1.0.5\" xmlns:ns30=\"urn://egisso-ru/types/package-LMSZ/1.0.5\" xmlns:ns31=\"urn://egisso-ru/types/local-MSZ/1.0.4\" xmlns:ns32=\"urn://egisso-ru/msg/10.05.I/1.0.5\" xmlns:ns33=\"urn://egisso-ru/types/package-LMSZ/1.0.5\" xmlns:ns34=\"urn://egisso-ru/types/local-MSZ/1.0.4\" xmlns:ns35=\"urn://egisso-ru/types/cls-KMSZ-version/1.0.2\" xmlns:ns36=\"urn://egisso-ru/types/cls-KMSZ-changes/1.0.2\" xmlns:ns37=\"urn://egisso-ru/types/basic/1.0.6\" xmlns:ns38=\"urn://egisso-ru/msg/10.03.O/1.0.1\" xmlns:ns39=\"urn://egisso-ru/msg/10.11.I/1.0.2\" xmlns:ns40=\"urn://egisso-ru/types/app-RU-OSZ/1.0.2\" xmlns:ns41=\"urn://egisso-ru/msg/10.11.I/1.0.2\" xmlns:ns42=\"urn://egisso-ru/types/app-RU-OSZ/1.0.2\" xmlns:ns43=\"urn://egisso-ru/msg/10.10.I/1.0.4\" xmlns:ns44=\"urn://egisso-ru/msg/10.10.I/1.0.4\" xmlns:ns45=\"urn://egisso-ru/types/cls-request/1.0.2\" xmlns:ns46=\"urn://egisso-ru/msg/10.02.I/1.0.2\" xmlns:ns47=\"urn://egisso-ru/types/cls-periodicity/1.0.0\" xmlns:ns48=\"urn://egisso-ru/types/cls-measure/1.0.2\" xmlns:ns49=\"urn://egisso-ru/msg/10.01.O/1.0.2\">");
                sbResult.Append($@"<ns10:package>
                                    <ns10:packageId>{packageId}</ns10:packageId>
                                    <ns10:elements>");
            }

            public string Bake()
            {
                sbResult.Append($@"   </ns10:elements>
                                    </ns10:package>
                                   </ns14:data>");

                var result = sbResult.ToString();

                return result;
            }

            public void AddFact(IEgissoFact fact)
            {
                var result = $@"<ns10:fact>
                                    <ns11:oszCode>{oszCode}</ns11:oszCode>
                                    {BuildReceiver(fact.Reciever)}
                                    {(fact.Reason != null ? BuildReason(fact.Reason) : string.Empty)}
                                    <ns11:lmszId>{fact.PrivilegeId}</ns11:lmszId>
                                    <ns11:categoryId>{fact.CategoryId}</ns11:categoryId>
                                    <ns11:decisionDate>{FormatDate(fact.DecisionDate)}</ns11:decisionDate>
                                    <ns11:dateStart>{FormatDate(fact.StartDate)}</ns11:dateStart>
                                    <ns11:dateFinish>{FormatDate(fact.EndDate)}</ns11:dateFinish>
                                    <ns11:needsCriteria>
                                      <ns11:usingSign>{FormatBool(fact.UsingNeedCriteria)}</ns11:usingSign>
                                      <ns11:criteria>{string.Empty}</ns11:criteria>
                                    </ns11:needsCriteria>
                                    <ns11:assignmentInfo>
                                      {BuildAssignment(fact)}
                                    </ns11:assignmentInfo>
                                    <ns10:uuid>{fact.Id}</ns10:uuid>
                              </ns10:fact>";

                sbResult.Append(result);
            }

            private string BuildReceiver(IEgissoFactReciever reciever)
            {
                var person = reciever.Person;

                return $@"<ns11:mszReceiver>
                                      <ns12:SNILS>{transform.Snils(person.Snils)}</ns12:SNILS>
                                      <FamilyName>{transform.Name(person.LastName)}</FamilyName>
                                      <FirstName>{transform.Name(person.FirstName)}</FirstName>
                                      <Patronymic>{transform.SurName(person.SurName)}</Patronymic>
                                      <ns12:Gender>{TransformGender(person.Gender)}</ns12:Gender>
                                      <ns12:BirthDate>{FormatDate(person.BirthDate)}</ns12:BirthDate>
                                      {BuildDocument(reciever.Document)}
                                    </ns11:mszReceiver>";
            }

            private string BuildReason(IEgissoFactReason reason)
            {
                var person = reason.Person;

                return $@"<ns11:reasonPersons>
                            <ns12:prsnInfo>
                                <ns12:SNILS>{transform.Snils(person.Snils)}</ns12:SNILS>
                                <FamilyName>{transform.Name(person.LastName)}</FamilyName>
                                <FirstName>{transform.Name(person.FirstName)}</FirstName>
                                <Patronymic>{transform.SurName(person.SurName)}</Patronymic>
                                <ns12:Gender>{TransformGender(person.Gender)}</ns12:Gender>
                                <ns12:BirthDate>{FormatDate(person.BirthDate)}</ns12:BirthDate>
                                {BuildDocument(reason.Document)}
                            </ns12:prsnInfo>
                          </ns11:reasonPersons>";
            }

            private string BuildDocument(IEgissoFactPersonDocument document)
            {
                if (!document.Valid)
                    return null;

                string type;
                string number = document.Number;
                string seria = document.Seria;
                switch (document.Type)
                {
                    case DocumentTypes.VID_NA_ZHITELSTVO:
                        type = "ns13:ResidencePermitRF";
                        break;
                    case DocumentTypes.TEMP_RF_PERSONID_FORM2P:
                        type = "ns13:TemporaryIdentityCardRF";
                        break;
                    case DocumentTypes.PASSPORT:
                        type = "ns13:PassportRF";
                        break;
                    case DocumentTypes.PASSPORT_FOREIGN:
                        type = "ns13:ForeignPassport";
                        break;
                    case DocumentTypes.BIRTH_CERTIFICATE:
                        type = "ns13:BirthCertificate";
                        seria = transform.BsSeria(seria);
                        break;
                    case DocumentTypes.MILITARY_PERSONID:
                        type = "ns13:MilitaryPassport";
                        break;
                    default:
                        type = "ns12:OtherDocument";
                        break;
                }

                return $@"<ns12:IdentityDoc>
                            <{type}>
                                <ns13:Series>{seria}</ns13:Series>
                                <ns13:Number>{number}</ns13:Number>
                                <ns13:IssueDate>{FormatDate(document.IssueDate)}</ns13:IssueDate>
                                <ns13:Issuer>{transform.DocIssuePlace(document.IssuePlace)}</ns13:Issuer>
                            </{type}>
                          </ns12:IdentityDoc>";
            }

            string BuildAssignment(IEgissoFactAssigment assigment)
            {
                string fAmount = assigment.IsDecimal ? assigment.Amount.ToString("0.00") : assigment.Amount.ToString("0");

                switch (assigment.ProvisionFormCode)
                {
                    case ProvisionFormCodes.MONETARY:
                        return $@"<ns11:monetaryForm>
                                    <ns11:amount>{fAmount}</ns11:amount>
                                  </ns11:monetaryForm>";
                    case ProvisionFormCodes.NATURAL:
                        return $@"<ns11:naturalForm>
                                    <ns11:amount>{fAmount}</ns11:amount>
                                    <ns11:measuryCode>{assigment.MeasureCode}</ns11:measuryCode>
                                    <ns11:content>{string.Empty}</ns11:content>
                                    <ns11:comment>{string.Empty}</ns11:comment>
                                  </ns11:naturalForm>";
                    case ProvisionFormCodes.EXEMPTION:
                        return $@"<ns11:exemptionForm>
                                    <ns11:amount>{fAmount}</ns11:amount>
                                    <ns11:measuryCode>{assigment.MeasureCode}</ns11:measuryCode>
                                    <ns11:monetization>{FormatBool(assigment.Monetization)}</ns11:monetization>
                                    <ns11:comment>{string.Empty}</ns11:comment>
                                  </ns11:exemptionForm>";
                    case ProvisionFormCodes.SERVICE:
                        return $@"<ns11:serviceForm>
                                    <ns11:amount>{fAmount}</ns11:amount>
                                    <ns11:measuryCode>{assigment.MeasureCode}</ns11:measuryCode>
                                    <ns11:content>{string.Empty}</ns11:content>
                                    <ns11:comment>{string.Empty}</ns11:comment>
                                  </ns11:serviceForm>";
                    default:
                        throw new InvalidOperationException();
                }
            }

            string FormatDate(DateTime date)
            {
                return date.ToString("yyyy-MM-ddzz:00");
            }

            string FormatBool(bool value)
            {
                return value ? "true" : "false";
            }

            private string TransformGender(char gender)
            {
                switch (gender)
                {
                    case 'м':
                    case 'М':
                        return "Male";
                    case 'ж':
                    case 'Ж':
                        return "Female";
                    default:
                        return null;
                }
            }
        }


    }

    public interface IEgissoEjectionContext
    {
        string ProviderCode { get; }
        string OszCode { get; }
    }

    public interface IEgissoFact: IEgissoFactAssigment
    {
        Guid Id { get; }
        Guid PrivilegeId { get; }
        Guid CategoryId { get; }
        bool UsingNeedCriteria { get; }

        DateTime DecisionDate { get; }
        DateTime StartDate { get; }
        DateTime EndDate { get; }

        IEgissoFactReciever Reciever { get; }
        IEgissoFactReason Reason { get; }
    }
    public interface IEgissoFactAssigment
    {
        string ProvisionFormCode { get; }
        decimal Amount { get; }
        bool IsDecimal { get; }
        string MeasureCode { get; }
        bool Monetization { get; }
    }

    public interface IEgissoFactReciever
    {
        IEgissoFactPerson Person { get; }
        IEgissoFactPersonDocument Document { get; }
    }
    public interface IEgissoFactReason
    {
        IEgissoFactPerson Person { get; }
        IEgissoFactPersonDocument Document { get; }
    }

    public interface IEgissoFactPerson
    {
        string LastName { get; }
        string FirstName { get; }
        string SurName { get; }
        char Gender { get; }
        DateTime BirthDate { get; }
        string Snils { get; }
    }
    public interface IEgissoFactPersonDocument
    {
        bool Valid { get; }

        string Type { get; }
        string Seria { get; }
        string Number { get; }
        DateTime IssueDate { get; }
        string IssuePlace { get; }
    }
}
