using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.Attributes.Validation
{
    public class MyDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)// Return a boolean value: true == IsValid, false != IsValid
        {
            DateTime d = Convert.ToDateTime(value);
            if (d == default(DateTime))
                return false;
            return d <= DateTime.Now; //Dates less than or equal to today are valid (true)

        }
    }
}
