using AisBenefits.Infrastructure.Services.DropDowns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AisBenefits.Infrastructure.Services.Egisso
{
    public class EjectionTransform
    {
        private StringBuilder sb = new StringBuilder();

        public string Name(string name)
        {
            if (IsSpaceChar(name[0]) || IsSpaceChar(name[name.Length - 1]))
            {
                sb.Clear();
                sb.Append(name);
                int rIndex;
                for (rIndex = sb.Length - 1; IsSpaceChar(sb[rIndex]); --rIndex) ;
                rIndex += 1;
                sb.Remove(rIndex, sb.Length - rIndex);

                for (rIndex = 0; IsSpaceChar(sb[rIndex]); ++rIndex) ;
                sb.Remove(0, rIndex);

                return sb.ToString();
            }
            return name;
        }
        bool IsSpaceChar(char c)
        {
            return c == ' ' || c == ' ';
        }
        public string SurName(string surName)
        {
            if (string.IsNullOrWhiteSpace(surName))
                return "-";
            return Name(surName);
        }
        public string Snils(string snils)
        {
            sb.Clear();
            sb.Append(snils);
            sb.Replace(" ", "");
            sb.Replace("-", "");
            return sb.ToString();
        }
        public string BsSeria(string docType, string seria)
        {
            if (docType == DocumentTypes.BIRTH_CERTIFICATE)
            {
                return BsSeria(seria);
            }
            return seria;
        }
        public string BsSeria(string seria)
        {
            if (seria == null || seria.Contains('-'))
                return seria;

            for (var i = 0; i < seria.Length; ++i)
            {
                var c = seria[i];
                if (c >= 'А' && c <= 'Я')
                {
                    sb.Clear();
                    sb.Append(seria);
                    sb.Insert(i, '-');
                    return sb.ToString();
                }
            }

            return seria;
        }
        public string DocIssuePlace(string issuePlace)
        {
            if (string.IsNullOrWhiteSpace(issuePlace))
                return "-";

            if (issuePlace.Contains(' '))
            {
                sb.Clear();
                sb.Append(issuePlace);
                sb.Replace(' ', ' ');
                issuePlace = sb.ToString();
            }

            if (issuePlace.Length > 200)
            {
                sb.Clear();
                sb.Append(issuePlace);
                sb.Remove(200, issuePlace.Length - 200);
                issuePlace = sb.ToString();
            }

            if (!Regex.IsMatch(issuePlace, @"^[а-яА-ЯёЁ\-0-9№(][а-яА-ЯёЁ\-\s'',.0-9()№]*$"))
            {
                issuePlace = string.Join(" ", Regex.Matches(issuePlace, @"[а-яА-ЯёЁ\-0-9№(][а-яА-ЯёЁ\-\s'',.0-9()№]*").Cast<Match>().Select(m => m.Value));
                if (string.IsNullOrWhiteSpace(issuePlace))
                    return "-";
            }
            return issuePlace;
        }
    }
}
