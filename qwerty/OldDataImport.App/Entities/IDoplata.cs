using System;
using System.Collections.Generic;
using System.Text;

namespace OldDataImport.App.Entities
{
    public interface IDoplata
    {
          string DELO { get; set; }
          int NUMBER { get; set; }
          string NPD { get; set; }
          string NPR { get; set; }
          string FM { get; set; }
          string IM { get; set; }
          string OT { get; set; }
          string POL { get; set; }
          DateTime DTR { get; set; }
          string PSR { get; set; }
          string PNM { get; set; }
          DateTime PSPDATA { get; set; }
          decimal GOSPENS { get; set; }
          decimal DOPPENS { get; set; }
          decimal RAZMVUPL { get; set; }
          DateTime SROKPO { get; set; }
          decimal SPOSOB { get; set; }
          string TIPRAION { get; set; }
          string RAION { get; set; }
          string PINDEX { get; set; }
          string TIPNSP { get; set; }
          string NSPNAME { get; set; }
          string TIPULICA { get; set; }
          string ULCNAME { get; set; }
          string DOM { get; set; }
          string KORPUS { get; set; }
          string KVARTIRA { get; set; }
          string PHONEDOM { get; set; }
          string PHONERAB { get; set; }
          string TIP { get; set; }
          string VARRASCHET { get; set; }
          string VDP { get; set; }
          string VDP_NAME { get; set; }
          string PERS { get; set; }
          string PERS_NAME { get; set; }
          string STAG { get; set; }
          decimal STAG_GOD { get; set; }
          decimal STAG_MES { get; set; }
          decimal STAG_DNI { get; set; }
          bool UTV_STAGA { get; set; }
          DateTime DATA_PRIEM { get; set; }
          DateTime DATA_UVOLN { get; set; }
          DateTime DATA_NAZN { get; set; }
          decimal OKLAD { get; set; }
          decimal UR_KOEF { get; set; }
          decimal OKLAD_UK { get; set; }
          decimal PREMIJA { get; set; }
          decimal PROCNADB { get; set; }
          decimal NADBAV { get; set; }
          decimal VARMATPOM { get; set; }
          decimal MATPOM { get; set; }
          decimal PROCVYSL { get; set; }
          decimal VYSLUGA { get; set; }
          decimal PROCKVAL { get; set; }
          decimal KVALIF { get; set; }
          decimal PERSEC { get; set; }
          decimal SECRET { get; set; }
          decimal SUMMA { get; set; }
          decimal PROCENT { get; set; }
          decimal DOPLATA { get; set; }
          decimal PENS_OBL { get; set; }
          decimal KOL_MINZRP { get; set; }
          decimal NR_ORGAN { get; set; }
          string ORGAN { get; set; }
          string DOLGN { get; set; }
          decimal NDOLGN { get; set; }
          decimal K_MDS { get; set; }
          decimal K_OKLAD { get; set; }
          decimal DOPL_OLD { get; set; }
          decimal UR_KOEF1 { get; set; }
          decimal OKLAD1 { get; set; }
          decimal OKLAD_UK1 { get; set; }
          decimal PROCENT1 { get; set; }
          decimal DOPL_NEW { get; set; }
          string ZAKON { get; set; }
          DateTime RASPRAV_DT { get; set; }
          string RASPRAV_NR { get; set; }
          string STAG_RP { get; set; }
          string STRANA { get; set; }
          string TIPREGION { get; set; }
          string REGION { get; set; }
          decimal KODRAIPENS { get; set; }
          string RAINAMEPEN { get; set; }
          string GOROD { get; set; }
          string DOCUMENT { get; set; }
          string NCARD { get; set; }
          bool CARD_HAND { get; set; }
          DateTime SDT { get; set; }
          DateTime FDT { get; set; }
          string NUM_REESTR { get; set; }
          string CITIZEN { get; set; }
          string MESTOROGD { get; set; }
          string PSPKEM { get; set; }
          decimal PROCPREM { get; set; }
          string SNILS { get; set; }
    }
}
