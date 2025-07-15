namespace Giel.Models
{
    public class NbpRateResponse
    {
        public string Table { get; set; }
        public string Currency { get; set; }
        public string Code { get; set; }
        public List<NbpRate> Rates { get; set; }
    }
}
