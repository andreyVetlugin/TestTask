using System;
using System.ComponentModel.DataAnnotations;

namespace OldDataImport.App.Entities
{
    public interface IPens
    {
        [MaxLength(2)]
        string RN { get; set; }

        [MaxLength(6)]
        string NPD { get; set; }
        
        [MaxLength(25)]
        string FM { get; set; }
        
        [MaxLength(20)]
        string IM { get; set; }
        
        [MaxLength(25)]
        string OT { get; set; }
        
        [MaxLength(10)]
        string DTR { get; set; }
        
        [MaxLength(11)]
        string TLF { get; set; }
        
        [MaxLength(75)]
        string ADR { get; set; }
        
        [MaxLength(6)]
        string DOM { get; set; }
        
        [MaxLength(3)]
        string KOR { get; set; }
        
        [MaxLength(4)]
        string KVR { get; set; }
        
        [MaxLength(5)]
        string PREKR { get; set; }
        
        DateTime PREKRSRS { get; set; }
        
        [MaxLength(25)]
        string PREKRNAPRI { get; set; }
        [DecimalLength(2)]
        [MaxLength(9)]
        decimal T_SUMP { get; set; }
        [DecimalLength(2)]
        [MaxLength(9)]
        decimal G_SUMP { get; set; }
        
        [MaxLength(4)]
        string ZOBRABD { get; set; }

    }

    public class Pens : IPens
    {
        public string RN { get; set; }
        public string NPD { get; set; }
        public string FM { get; set; }
        public string IM { get; set; }
        public string OT { get; set; }
        public string DTR { get; set; }
        public string TLF { get; set; }
        public string ADR { get; set; }
        public string DOM { get; set; }
        public string KOR { get; set; }
        public string KVR { get; set; }
        public string PREKR { get; set; }
        public DateTime PREKRSRS { get; set; }
        public string PREKRNAPRI { get; set; }
        public decimal T_SUMP { get; set; }
        public decimal G_SUMP { get; set; }
        public string ZOBRABD { get; set; }
    }

    public class DecimalLengthAttribute : Attribute
    {
        public DecimalLengthAttribute(int length)
        {
            Length = length;
        }

        public int Length { get; }

    }
}
