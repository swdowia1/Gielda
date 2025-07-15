using Giel.Data;
using Giel.Models;
using Microsoft.EntityFrameworkCore;
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
        public async Task<List<CurrencyRate>> GetAllRatesAsync()
        {
            return await _context.CurrencyRates
                .OrderByDescending(r => r.Date)
                .ToListAsync();
        }
        public async Task FetchTodayUsdRateAsync()
        {
            string url = "https://api.nbp.pl/api/exchangerates/rates/A/USD/today/?format=json";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Nie udało się pobrać kursu.");
                return;
            }

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<NbpRateResponse>(json);

            var rate = data?.Rates?.FirstOrDefault();
            if (rate == null)
            {
                Console.WriteLine("Brak danych kursu.");
                return;
            }

            var currencyRate = new CurrencyRate
            {
                CurrencyCode = data.Code,
                Rate = rate.Mid,
                Date = DateTime.Parse(rate.EffectiveDate)
            };

            // Opcjonalnie: sprawdź czy taki kurs już istnieje (żeby nie dublować)
            var exists = await _context.CurrencyRates
                .AnyAsync(r => r.CurrencyCode == currencyRate.CurrencyCode && r.Date == currencyRate.Date);

            if (!exists)
            {
                _context.CurrencyRates.Add(currencyRate);
                await _context.SaveChangesAsync();
                Console.WriteLine("Kurs zapisano do bazy.");
            }
            else
            {
                Console.WriteLine("Kurs z tego dnia już istnieje w bazie.");
            }
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
