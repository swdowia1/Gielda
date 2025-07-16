using Giel.Data;
using Giel.Models;

namespace Giel.BackGround
{
    public class BackgroundRateService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public BackgroundRateService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    var newRate = new CurrencyRate
                    {
                        Rate = GetRandomRate(), // lub pobierz z API
                        Date = DateTime.UtcNow,
                        CurrencyCode="USD"
                    };

                    dbContext.CurrencyRates.Add(newRate);
                    await dbContext.SaveChangesAsync();
                }

                await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken);
            }
        }

        private decimal GetRandomRate()
        {
            var rnd = new Random();
            return (decimal)(rnd.NextDouble() * 100); // przykładowa losowa wartość
        }
    }
}
