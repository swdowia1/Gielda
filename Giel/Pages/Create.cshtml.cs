using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Giel.Data;
using Giel.Models;

namespace Giel.Pages
{
    public class CreateModel : PageModel
    {
        private readonly Giel.Data.AppDbContext _context;

        public CreateModel(Giel.Data.AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public CurrencyRate CurrencyRate { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.CurrencyRates.Add(CurrencyRate);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
