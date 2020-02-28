using System.Text;
using System.Text.RegularExpressions;

namespace AisBenefits.Infrastructure.Helpers
{
    public class SnilsHelper
    {
        public static string Normalize(string snils)
        {
            if (IsNormaized(snils))
                return snils;
            return snils.Trim().Replace(' ', '-');
        }

        public static string Normalize(string snils, StringBuilder stringBuilder)
        {
            if (IsNormaized(snils))
                return snils;
            stringBuilder.Clear();
            stringBuilder.Append(snils);
            stringBuilder[11] = '-';
            return stringBuilder.ToString();
        }

        public static bool IsNormaized(string snils)
        {
            return snils.Length > 11 && snils[11] == '-';
        }

        public static bool IsValid(string snils)
        {
            return Regex.Match(snils, @"\d{3}-\d{3}-\d{3}[-\s]\d{2}").Success;
        }
        public static bool IsValidChecksum(string snils)
        {
            snils = snils.Replace("-", "").Replace(" ", "");
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
