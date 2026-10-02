using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AvenaWeb.Pages;

public class LogoutModel : PageModel
{
    private const string AuthenticationScheme = "AvenaCookie";

    public async Task<IActionResult> OnPostAsync()
    {
        await HttpContext.SignOutAsync(AuthenticationScheme);
        return RedirectToPage("/Index");
    }

    // Handles a direct /Logout visit as well, so it can never render the login page.
    public async Task<IActionResult> OnGetAsync()
    {
        await HttpContext.SignOutAsync(AuthenticationScheme);
        return RedirectToPage("/Index");
    }
}
