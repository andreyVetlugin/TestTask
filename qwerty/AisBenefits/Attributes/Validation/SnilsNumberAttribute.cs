using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AisBenefits.Attributes.Validation
{
    public class SnilsNumberAttribute : ValidationAttribute
    {
        //private const string SNILS_FORMAT_PATTERN = "^([0-9]{3}-){3}[0-9]{2}$";
        private const string SNILS_FORMAT_PATTERN = @"^\d{3}-\d{3}-\d{3} \d{2}$";

        private const string FORMAT_ERROR_MSG = "Номер введен в неверном формате";
        private const string CHECKSUM_ERROR_MSG = "Ошибка контрольной суммы";
        private const string NULL_ERROR_MSG = "Не введён СНИЛС";


        public override bool IsValid(object value)
        {

            if (!(value is string snils))
            {
                ErrorMessage = NULL_ERROR_MSG;
                return false;
            }


            if (!Regex.IsMatch(snils, SNILS_FORMAT_PATTERN))
            {
                ErrorMessage = FORMAT_ERROR_MSG;
                return false;
            }
            if (!ValidateSnilsNumber(snils))
            {
                ErrorMessage = CHECKSUM_ERROR_MSG;
                return false;
            }
            return true;
        }


        private bool ValidateSnilsNumber(string value)
        {
            var snils = value.Replace("-", "").Replace(" ", "");
            var control = int.Parse(snils.Substring(9, 2));
            var number = int.Parse(snils.Substring(0, 9));
            if (number <= 1001998)
            {
                return true;
            }

            int sum = 0;
            for (int i = 0; i < 9; ++i)
                sum += int.Parse(snils[i].ToString()) * (9 - i);

            var checksum = sum;
            do
            {
                if (checksum == 100 || checksum == 101)
                {
                    return control == 0;
                }
                checksum %= 101;
            } while (checksum > 99);

            return control == checksum;
        }


       
    }
}
