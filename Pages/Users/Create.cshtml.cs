using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using llm.Data;
using llm.Models;

namespace llm.Pages.Users
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public User UserModel { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Check if email already exists
            if (await _context.Users.AnyAsync(u => u.Email == UserModel.Email))
            {
                ModelState.AddModelError("UserModel.Email", "Email already exists.");
                return Page();
            }

            UserModel.CreatedAt = DateTime.UtcNow;
            _context.Users.Add(UserModel);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
