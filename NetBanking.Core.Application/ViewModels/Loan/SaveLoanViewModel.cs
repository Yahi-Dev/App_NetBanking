using System.ComponentModel.DataAnnotations;

namespace NetBanking.Core.Application.ViewModels.Loan
{
    public class SaveLoanViewModel
    {
        public string? Id { get; set; }
        public string UserId { get; set; }

        [Required(ErrorMessage = "La cantidad es requerida")]
        public decimal Amount { get; set; }
        [Required(ErrorMessage = "El límite aprobado es requerida")]
        public decimal Debt { get; set; }
        public bool HasError { get; set; }
        public string? Error { get; set; }
        public string CreatedById { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
