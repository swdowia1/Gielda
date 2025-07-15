using Giel.Data;
using Giel.Services;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
var culture = new CultureInfo("pl-PL");
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient<NbpApiService>();
builder.Services.AddScoped<NbpApiService>();
builder.Services.AddScoped<PredictionService>();
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
//using (var scope = app.Services.CreateScope())
//{
//    var service = scope.ServiceProvider.GetRequiredService<NbpApiService>();
//    await service.FetchUsdRatesLastMonthAsync();
//}

app.Run();
