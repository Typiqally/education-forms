using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Summa.Forms.Identity.Pages.Account
{
    public class Index : PageModel
    {
        public IActionResult OnGet()
        {
            return Redirect("https://localhost:5001");
        }
    }
}