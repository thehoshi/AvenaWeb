using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AvenaWeb.Pages;

public class RegisterModel : PageModel
{
    public IActionResult OnGet() => RedirectToPage("/Login");
    public IActionResult OnPost() => RedirectToPage("/Login");
}
