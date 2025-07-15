using Giel.Data;
using Giel.Models;
using Newtonsoft.Json;
using System.Globalization;

namespace Giel.Services
{
    public class NbpApiService
    {
        private readonly HttpClient _httpClient;
        private readonly AppDbContext _context;

        public NbpApiService(HttpClient httpClient, AppDbContext context)
        {
            _httpClient = httpClient;
            _context = context;
        }

        public async Task FetchUsdRatesLastMonthAsync()
        {
            var endDate = DateTime.Today;
            var startDate = endDate.AddDays(-30);
            string url = $"https://api.nbp.pl/api/exchangerates/rates/A/USD/{startDate:yyyy-MM-dd}/{endDate:yyyy-MM-dd}/?format=json";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) return;

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<NbpRateResponse>(json);

            foreach (var rate in data.Rates)
            {
                var date = DateTime.ParseExact(rate.EffectiveDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                if (!_context.CurrencyRates.Any(r => r.CurrencyCode == "USD" && r.Date == date))
                {
                    _context.CurrencyRates.Add(new CurrencyRate
                    {
                        CurrencyCode = "USD",
                        Rate = rate.Mid,
                        Date = date
                    });
                }
            }

            await _context.SaveChangesAsync();
        }

    }
}
