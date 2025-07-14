namespace Giel.Data
{
    using Microsoft.EntityFrameworkCore;

    using Giel.Models;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<CurrencyRate> CurrencyRates { get; set; }
    }
}
