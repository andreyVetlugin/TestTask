using System.ComponentModel.DataAnnotations;

namespace AisBenefits.Models.Authorize
{
    public class AuthForm
    {
        [Required(ErrorMessage = "Не указано")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Не указано")]
        public string Password { get; set; }
    }
}
