using DataLayer.Misc;
using System;
using System.Collections.Generic;
using System.Text;

namespace AisBenefits.Infrastructure.Services.DropDowns
{
    public static class DocumentTypes
    {
        public const string VID_NA_ZHITELSTVO = "01";
        public const string TEMP_RF_PERSONID_FORM2P = "02";
        public const string PASSPORT = "03";
        public const string PASSPORT_FOREIGN = "04";
        public const string BIRTH_CERTIFICATE = "05";
        public const string REFUGEE_PERSONID = "06";
        public const string MILITARY_PERSONID = "07";
        public const string OTHER_DOCUMENTS = "08";
        public const string NONE = "00";

        public static Dictionary<string, string> documentTypes = new Dictionary<string, string>()
        {
            { VID_NA_ZHITELSTVO, "Вид на жительство" },
            { TEMP_RF_PERSONID_FORM2P, "Временное удостоверение личности гражданина РФ по форме 2П" },
            { PASSPORT, "Паспорт гражданина РФ" },
            { PASSPORT_FOREIGN, "Паспорт иностранного гражданина" },
            { BIRTH_CERTIFICATE, "Свидетельство о рождении"},
            { REFUGEE_PERSONID, "Удостоверение беженцев" },
            { MILITARY_PERSONID, "Удостоверение личности военнослужащего РФ" },
            { OTHER_DOCUMENTS, "Иные документы" },
            { NONE, "Не выбрано" },
        };
    }
}
