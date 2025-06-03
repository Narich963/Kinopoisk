using System.ComponentModel.DataAnnotations;

namespace Kinopoisk.MVC.Models.Accounts;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Username is required.")]
    public string Username { get; set; }

    [DataType(DataType.Password)]   
    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Confirm Password is required.")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
}
