using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Giel.Data;
using Giel.Models;
using Giel.Services;

namespace Giel.Pages
{
    public class IndexModel : PageModel
    {
       
        private NbpApiService nbpApiService;

        public IndexModel(NbpApiService nbpApiService)
        {
            this.nbpApiService = nbpApiService;
        }

        public IList<CurrencyRate> CurrencyRate { get;set; } = default!;

        public async Task OnGetAsync()
        {
            CurrencyRate = await nbpApiService.GetAllRatesAsync();
        }
        public async Task<IActionResult> OnPostFetchTodayRateAsync()
        { 
            await nbpApiService.FetchTodayUsdRateAsync();
            return RedirectToPage(); 
        }
     }
}
