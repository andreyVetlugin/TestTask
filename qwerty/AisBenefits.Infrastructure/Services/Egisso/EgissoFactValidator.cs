using AisBenefits.Infrastructure.Helpers;
using AisBenefits.Infrastructure.Services.DropDowns;
using DataLayer.Entities;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AisBenefits.Infrastructure.Services.Egisso
{
    public static class EgissoFactValidator
    {
        public static EgissoFactValidationResult ValidateEgissoFact<TContext>(this TContext context, PersonInfo personInfo)
            where TContext: EjectionTransform
        {
            List<string> errors = new List<string>();

            context.ValidateDocumnet(errors, personInfo);

            var documentValid = errors.Count == 0;
            // Подробности валидации документа не требуются - документ не обязателен для выгрузки
            // Нужен только флажок, что документ не валиден, чтобы не выгружать его
            errors.Clear();

            context.ValidatePerson(errors, personInfo);

            return new EgissoFactValidationResult(errors, documentValid);
        }

        static void ValidatePerson<TContext>(this TContext context, List<string> errors, PersonInfo person)
            where TContext : EjectionTransform
        {
            if (string.IsNullOrWhiteSpace(person.SNILS))
                errors.Add($"Не указан СНИЛС");
            else if (!SnilsHelper.IsValid(person.SNILS))
                errors.Add($"Некорректный формат СНИЛС");
            else if (!SnilsHelper.IsValidChecksum(person.SNILS))
                errors.Add($"Неверная контрольная сумма СНИЛС");

            if (string.IsNullOrEmpty(person.SurName) || !ValidName(context.Name(person.SurName)))
                errors.Add($"Некорректная фамилия");
            if (string.IsNullOrEmpty(person.Name) || !ValidName(context.Name(person.Name)))
                errors.Add($"Некорректное имя");
            if (!string.IsNullOrEmpty(person.MiddleName) && !ValidName(context.Name(person.MiddleName)))
                errors.Add($"Некорректное отчество");
        }

        static void ValidateDocumnet<TContext>(this TContext context, List<string> errors, PersonInfo document)
            where TContext: EjectionTransform
        {
            if (string.IsNullOrWhiteSpace(document.DocTypeId))
            {
                errors.Add($"Не указан документ");
            }
            else if (document.DocTypeId == DocumentTypes.BIRTH_CERTIFICATE)
            {
                if (string.IsNullOrWhiteSpace(document.DocSeria))
                    errors.Add($"Не указана серия свидетельства о рождении");
                else if (!Regex.IsMatch(document.DocSeria, "^[IVXLCDM]{1,3}-?[А-Я]{2}$"))
                    errors.Add($"Некорректная серия свидельства о рождении");

                if (!Regex.IsMatch(document.DocNumber, @"^\d{6}$"))
                    errors.Add($"Некорректный номер свидетельства о рождении");
            }
            else if (document.DocTypeId == DocumentTypes.PASSPORT)
            {
                if (string.IsNullOrWhiteSpace(document.DocSeria))
                    errors.Add($"Не указана серия паспорта");
                else if (!Regex.IsMatch(document.DocSeria, @"^\d{4}$"))
                    errors.Add($"Некорректная серия паспорта");

                if (!Regex.IsMatch(document.DocNumber, @"^\d{6}$"))
                    errors.Add($"Некорректный номер паспорта");
            }
            else if (document.DocTypeId == DocumentTypes.PASSPORT_FOREIGN)
            {
                if (!Regex.IsMatch(document.DocNumber, @"^[0-9а-яА-ЯA-Za-z]{1,25}$"))
                    errors.Add($"Некорректный номер иностранного документа");
            }
        }

        static bool ValidName(string name)
        {
            return Regex.IsMatch(name, @"^[а-яА-ЯёЁ\-0-9][а-яА-ЯёЁ\-\s'',.]*$");
        }
    }

    public struct EgissoFactValidationResult
    {
        public readonly List<string> Errors;
        public readonly bool DocumentValid;
        public bool Ok => Errors.Count == 0;

        public EgissoFactValidationResult(List<string> errors, bool documentValid)
        {
            Errors = errors;
            DocumentValid = documentValid;
        }
    }
}
