using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace NetBanking.Core.Application.ViewModels.Users
{
    public class SaveUserViewModel
    {
        public bool? IsActive {  get; set; }
        public string? Id { get; set; }

        [Required(ErrorMessage = "Please enter a IdCard for your user")]
        public  string IdCard { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? formFile { get; set; }
        public string? Role {  get; set; }
        public string? ImageURL { get; set; }

        [Required(ErrorMessage = "Please enter a First Name")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter a LastName")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please entar a UserName")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).+$"
        , ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Passwords does not match")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please type in an email address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please type in a Phone number")]
        [DataType(DataType.Text)]
        public string PhoneNumber { get; set; }
        public decimal InitialAmount { get; set; }

        [Required(ErrorMessage = "You have to type in an inital amout for your first bank account")]
        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
