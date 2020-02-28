using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.Attributes.Validation
{
    public class MoneyAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)// Return a boolean value: true == IsValid, false != IsValid
        {
            var d = Convert.ToDouble(value);
            
            return d >= 0; //Возвращает тру, если число положительное

        }
    }
}
