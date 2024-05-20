using NetBanking.Core.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace NetBanking.Core.Application.ViewModels.Transaction
{
    public class SaveTransactionViewModel
    {
        public string? Id { get; set; }
        [Required(ErrorMessage ="El producto emisor es requerido")]
        [DataType(DataType.Text)]
        public string EmissorProductId { get; set; }
        [Required(ErrorMessage = "El producto receptor es requerido")]
        [DataType(DataType.Text)]
        public string ReceiverProductId { get; set; }

        [Required(ErrorMessage = "La cantidad es requerida")]
        public decimal Cantity { get; set; }
        public TransactionType Type { get; set; }
        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
