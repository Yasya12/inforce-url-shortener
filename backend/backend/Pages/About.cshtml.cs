using backend.Data;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace backend.Pages
{
    public class AboutModel(ApplicationDbContext context) : PageModel
    {
        private readonly ApplicationDbContext _context = context;

        [BindProperty]
        public required AboutContent AboutContent { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            AboutContent = await _context.AboutContents.FirstOrDefaultAsync()
                ?? new AboutContent { Id = 1, Content = "" };

            var dbPath = _context.Database.GetDbConnection().DataSource;
            Console.WriteLine("DB path: " + dbPath);

            var allContents = await _context.AboutContents.ToListAsync();

            Console.WriteLine("allContents: " + AboutContent);
            return Page();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(AboutContent).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Зміни успішно збережені!";

            return RedirectToPage("./About");
        }
    }
}