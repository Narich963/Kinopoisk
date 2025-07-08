using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kinopoisk.MVC.Pages;

public class SetLanguageModel : PageModel
{
    public IActionResult OnPost(string culture, string returnUrl)
    {
        if (string.IsNullOrEmpty(culture))
            culture = "en";
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddDays(30) });

        return LocalRedirect(returnUrl);
    }
}
