using backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class LogoutModel(SignInManager<ApplicationUser> signInManager) : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;

    public async Task<IActionResult> OnGetAsync()
    {
        await _signInManager.SignOutAsync();
        return Redirect("./browser/logout"); 
    }
}
