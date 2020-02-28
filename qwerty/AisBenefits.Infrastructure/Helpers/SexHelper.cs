namespace AisBenefits.Infrastructure.Helpers
{
    public static class SexHelper
    {
        public static bool IsMale(char sex)
        {
            return sex == 'м' || sex == 'М';
        }

        public static bool IsFemale(char sex)
        {
            return sex == 'ж' || sex == 'Ж';
        }
    }

    public static class Sex
    {
        public const char MALE = 'м';
        public const char FEMALE = 'ж';
    }
}
