using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.Attributes.Validation
{
    public class ValidGuidAttribute : ValidationAttribute
    {        


        public override bool IsValid(object value)
        {
            var input = Convert.ToString(value);

            
            if (string.IsNullOrWhiteSpace(input))
            {
                ErrorMessage = "Не введён Guid";
                return false;
            }
            

            if (!Guid.TryParse(input, out Guid guid))
            {                
                ErrorMessage = "Это не Guid";
                return false;
            }


            return true;

        }
    }
}
