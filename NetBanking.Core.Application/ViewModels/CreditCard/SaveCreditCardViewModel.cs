using System.ComponentModel.DataAnnotations;

namespace NetBanking.Core.Application.ViewModels.CreditCard
{
    public class SaveCreditCardViewModel
    {
        public string? Id { get; set; }
        public string UserId { get; set; }
        [Required(ErrorMessage = "El límite aprobado es requerida")]
        public decimal Limit { get; set; }
        public decimal Amount { get; set; }
        public bool HasError { get; set; }
        public string? Error { get; set; }
        public string? CreatedById { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
