using DataLayer.Misc;
using System;
using System.Collections.Generic;
using System.Text;

namespace AisBenefits.Infrastructure.Services.PersonInfos
{
    public static class EgissoCodeGetService
    {
        public const string VID_NA_ZHITELSTVO = "01";
        public const string TEMP_RF_PERSONID_FORM2P = "02";
        public const string PASSPORT = "03";
        public const string PASSPORT_FOREIGN = "04";
        public const string BIRTH_CERTIFICATE = "05";
        public const string REFUGEE_PERSONID = "06";
        public const string MILITARY_PERSONID = "07";
        public const string OTHER_DOCUMENTS = "08";


        public static string GetEgissoCode (string documentType)
        {
            switch (documentType)
            {
                case PersonDocumentTypes.VID_NA_ZHITELSTVO:
                    {
                        return VID_NA_ZHITELSTVO;
                    };
                case PersonDocumentTypes.TEMP_RF_PERSONID_FORM2P:
                    {
                        return TEMP_RF_PERSONID_FORM2P;
                    };
                case PersonDocumentTypes.PASSPORT:
                    {
                        return PASSPORT;
                    };
                case PersonDocumentTypes.PASSPORT_FOREIGN:
                    {
                        return PASSPORT_FOREIGN;
                    };
                case PersonDocumentTypes.BIRTH_CERTIFICATE:
                    {
                        return BIRTH_CERTIFICATE;
                    };
                case PersonDocumentTypes.REFUGEE_PERSONID:
                    {
                        return REFUGEE_PERSONID;
                    };
                case PersonDocumentTypes.MILITARY_PERSONID:
                    {
                        return MILITARY_PERSONID;
                    };
                case PersonDocumentTypes.OTHER_DOCUMENTS:
                    {
                        return OTHER_DOCUMENTS;
                    };
                default:
                    //return string.Empty;
                    throw new FormatException();

            }           
        }
    }
}
