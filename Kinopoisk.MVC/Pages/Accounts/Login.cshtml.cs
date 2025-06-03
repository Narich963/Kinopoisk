using Kinopoisk.Core.Enitites;
using Kinopoisk.MVC.Models.Accounts;
using Kinopoisk.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kinopoisk.MVC.Pages.Accounts;

public class LoginModel : PageModel
{
    private readonly SignInManager<User> _signInManager;
    private readonly IUserService _userService;

    public LoginModel(SignInManager<User> signInManager, IUserService userService)
    {
        _signInManager = signInManager;
        _userService = userService;
    }

    [BindProperty]
    public LoginViewModel LoginViewModel { get; set; } = new();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        if (!await _userService.IsExistingUser(LoginViewModel.Login))
        {
            ModelState.AddModelError(string.Empty, "Invalid login or password.");
            return Page();
        }
        var result = await _signInManager.PasswordSignInAsync(LoginViewModel.Login, LoginViewModel.Password, true, false);
        if (result.Succeeded)
        {
            return RedirectToPage("/Films/Index");
        }
        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return Page();
    }
}
