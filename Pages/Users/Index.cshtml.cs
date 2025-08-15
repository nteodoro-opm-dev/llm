using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using llm.Data;
using llm.Models;

namespace llm.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<User> UsersList { get; set; } = default!;

        public async Task OnGetAsync()
        {
            UsersList = await _context.Users
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();
        }
    }
}
