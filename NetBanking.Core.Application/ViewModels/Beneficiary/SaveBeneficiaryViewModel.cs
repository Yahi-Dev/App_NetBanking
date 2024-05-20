using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace NetBanking.Core.Application.ViewModels.Beneficiary
{
    public class SaveBeneficiaryViewModel
    {
        public string? Id { get; set; }
        public string UserId { get; set; }
        public string? BeneficiaryId { get; set; }

        [Required(ErrorMessage = "Hace falta la cuenta beneficiaria")]
        [DataType(DataType.Text)]       
        public string BeneficiaryAccountId { get; set; }
        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
