using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Giel.Data;
using Giel.Models;

namespace Giel.Pages
{
    public class EditModel : PageModel
    {
        private readonly Giel.Data.AppDbContext _context;

        public EditModel(Giel.Data.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CurrencyRate CurrencyRate { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currencyrate =  await _context.CurrencyRates.FirstOrDefaultAsync(m => m.Id == id);
            if (currencyrate == null)
            {
                return NotFound();
            }
            CurrencyRate = currencyrate;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(CurrencyRate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CurrencyRateExists(CurrencyRate.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool CurrencyRateExists(int id)
        {
            return _context.CurrencyRates.Any(e => e.Id == id);
        }
    }
}
