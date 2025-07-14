using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Giel.Data;
using Giel.Models;

namespace Giel.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly Giel.Data.AppDbContext _context;

        public DetailsModel(Giel.Data.AppDbContext context)
        {
            _context = context;
        }

        public CurrencyRate CurrencyRate { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currencyrate = await _context.CurrencyRates.FirstOrDefaultAsync(m => m.Id == id);
            if (currencyrate == null)
            {
                return NotFound();
            }
            else
            {
                CurrencyRate = currencyrate;
            }
            return Page();
        }
    }
}
