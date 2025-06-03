using System.ComponentModel.DataAnnotations;

namespace Kinopoisk.MVC.Models.Accounts;

public class LoginViewModel
{
    [Required(ErrorMessage = "Enter Username or Email")]
    [Display(Name = "Username or Email")]
    public string Login { get; set; }

    [Required(ErrorMessage = "Enter Password")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }
}
