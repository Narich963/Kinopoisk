using Kinopoisk.Core.Enitites;
using Kinopoisk.MVC.Models.Accounts;
using Kinopoisk.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kinopoisk.MVC.Pages.Accounts;

public class RegisterModel : PageModel
{
    private readonly IUserService _userService;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public RegisterModel(IUserService userService, SignInManager<User> signInManager, UserManager<User> userManager)
    {
        _userService = userService;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [BindProperty]
    public RegisterViewModel RegisterViewModel { get; set; } = new();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        if (await _userService.IsExistingUser(RegisterViewModel.Username))
        {
            ModelState.AddModelError(string.Empty, "That username is already taken.");
            return Page();
        }
        var user = new User
        {
            UserName = RegisterViewModel.Username
        };
        var result = await _userManager.CreateAsync(user, RegisterViewModel.Password);
        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToPage("/Films/Index");
        }

        ModelState.AddModelError(string.Empty, "Registration failed. Please try again.");
        return Page();
    }
}
