using System.ComponentModel.DataAnnotations;

namespace NetBanking.Core.Application.ViewModels.Users
{
    public class ForgorPasswordViewModel
    {
        [Required(ErrorMessage = "Please type in an email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
