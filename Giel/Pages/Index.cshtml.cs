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
    public class IndexModel : PageModel
    {
        private readonly Giel.Data.AppDbContext _context;

        public IndexModel(Giel.Data.AppDbContext context)
        {
            _context = context;
        }

        public IList<CurrencyRate> CurrencyRate { get;set; } = default!;

        public async Task OnGetAsync()
        {
            CurrencyRate = await _context.CurrencyRates.ToListAsync();
        }
    }
}
