using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer.Entities.EGISSO
{
    public class ProvisionForm: IBenefitsEntity
    {
       
        public Guid Id { get; set; }

        public string Title { get; set; }
        [MaxLength(2)]
        public string Code { get; set; }
    }

    public static class ProvisionFormCodes
    {
        public const string MONETARY = "01";
        public const string NATURAL = "02";
        public const string EXEMPTION = "03";
        public const string SERVICE = "04";
    }
}
