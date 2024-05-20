using System.ComponentModel.DataAnnotations;

namespace NetBanking.Core.Application.ViewModels.Users
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "You should enter a email address")]
        [DataType(DataType.Text)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please type in a password")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).+$"
        , ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "The passwords does not match")]
        [Required(ErrorMessage = "You should entar a password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public string? Token { get; set; }
        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
