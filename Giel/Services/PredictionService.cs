using Giel.Data;
using Giel.Models;
using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;

namespace Giel.Services
{
    public class PredictionService
    {
        private readonly AppDbContext _context;
        public PredictionService(AppDbContext context)
        {
            _context = context;
        }
        public List<(DateTime Date, float Forecast)> PredictUsd(int days = 7)
        {
            var mlContext = new MLContext();

            var rates = _context.CurrencyRates
                .Where(x => x.CurrencyCode == "USD")
                .OrderBy(x => x.Date)
                .Select(x => new CurrencyRateData { Rate = (float)x.Rate })
                .ToList();

            var data = mlContext.Data.LoadFromEnumerable(rates);

            var pipeline = mlContext.Forecasting.ForecastBySsa(
                outputColumnName: nameof(CurrencyRateForecast.ForecastedRate),
                inputColumnName: nameof(CurrencyRateData.Rate),
                windowSize: 7,
                seriesLength: rates.Count,
                trainSize: rates.Count,
                horizon: days,
                confidenceLevel: 0.95f,
                confidenceLowerBoundColumn: nameof(CurrencyRateForecast.LowerBoundRate),
                confidenceUpperBoundColumn: nameof(CurrencyRateForecast.UpperBoundRate));

            var model = pipeline.Fit(data);

            var forecastingEngine = model.CreateTimeSeriesEngine<CurrencyRateData, CurrencyRateForecast>(mlContext);

           // var forecast = forecastingEngine.Predict();
            var forecast = forecastingEngine.Predict(days);
            var lastDate = _context.CurrencyRates
                .Where(x => x.CurrencyCode == "USD")
                .Max(x => x.Date);

            var result = new List<(DateTime Date, float Forecast)>();

            for (int i = 0; i < forecast.ForecastedRate.Length; i++)
            {
                result.Add((lastDate.AddDays(i + 1), forecast.ForecastedRate[i]));
            }

            return result;
        }
    }
}
