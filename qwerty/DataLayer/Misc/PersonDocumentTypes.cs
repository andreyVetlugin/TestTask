using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Misc
{
    public static class PersonDocumentTypes
    {
        public const string VID_NA_ZHITELSTVO = "Вид на жительство";
        public const string TEMP_RF_PERSONID_FORM2P = "Временное удостоверение личности гражданина РФ по форме 2П";
        public const string PASSPORT = "Паспорт гражданина РФ";
        public const string PASSPORT_FOREIGN = "Паспорт иностранного гражданина";
        public const string BIRTH_CERTIFICATE = "Свидетельство о рождении";
        public const string REFUGEE_PERSONID = "Удостоверение беженцев";
        public const string MILITARY_PERSONID = "Удостоверение личности военнослужащего РФ";
        public const string OTHER_DOCUMENTS = "Иные документы";
        public const string NONE = "Не выбрано";


        public static string[] AllTypes = new[]
        {
            VID_NA_ZHITELSTVO,
            TEMP_RF_PERSONID_FORM2P,
            PASSPORT,
            PASSPORT_FOREIGN,
            BIRTH_CERTIFICATE,
            REFUGEE_PERSONID,
            MILITARY_PERSONID,
            OTHER_DOCUMENTS,
            NONE
        };


    }

}
