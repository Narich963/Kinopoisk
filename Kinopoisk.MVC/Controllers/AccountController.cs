using Kinopoisk.Core.Enitites;
using Kinopoisk.MVC.Models;
using Kinopoisk.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Kinopoisk.MVC.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<User> _signInManager;
    private readonly IUserService _userService;

    public AccountController(SignInManager<User> signInManager, IUserService userService)
    {
        _signInManager = signInManager;
        _userService = userService;
    }

    [HttpGet]
    public IActionResult Login() => View(new LoginViewModel());

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (!await _userService.IsExistingUser(model.Login))
        {
            ModelState.AddModelError(string.Empty, "Invalid login or password.");
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(model.Login, model.Password, true, false);
        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Films");
        }
        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View(model);
    }
}
