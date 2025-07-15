using Giel.Data;
using Giel.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Giel.Pages
{
    public class PredyktModel : PageModel
    {
        private readonly PredictionService _predictionService;

        private readonly AppDbContext _context;
        public List<string> Labels { get; set; } = new();
        public List<float> HistoryRates { get; set; } = new();
        public List<float> ForecastRates { get; set; } = new();


        public PredyktModel(PredictionService predictionService, AppDbContext context)
        {
            _predictionService = predictionService;
            _context = context;
        }



        public void OnGet()
        {
            int ilosc = 10;
            // 1. Dane historyczne (np. 14 dni)
            var history = _context.CurrencyRates
                .Where(x => x.CurrencyCode == "USD")
                .OrderBy(x => x.Date)
               .OrderByDescending(x => x.Date)
.Take(ilosc)
.OrderBy(x => x.Date)
                .ToList();

            HistoryRates = history.Select(x => (float)x.Rate).ToList();

            // 2. Predykcja na kolejne 7 dni
            var predictions = _predictionService.PredictUsd(14);

            ForecastRates = predictions.Select(x => x.Forecast).ToList();

            // 3. Po³¹cz etykiety
            var allDates = history.Select(x => x.Date)
                .Concat(predictions.Select(x => x.Date))
                .Select(d => d.ToString("yyyy-MM-dd"))
                .ToList();

            Labels = allDates;
        }
    }
}
