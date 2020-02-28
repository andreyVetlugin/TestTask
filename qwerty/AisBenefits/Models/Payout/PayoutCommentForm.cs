using System;
using System.ComponentModel.DataAnnotations;

namespace AisBenefits.Models.Payout
{
    public class PayoutCommentForm
    {
        public Guid PayoutId { get; set; }

        [Required(ErrorMessage = "Не указан"), MaxLength(256, ErrorMessage = "Должно быть не больше 256 символов")]
        public string Comment { get; set; }
    }
}
