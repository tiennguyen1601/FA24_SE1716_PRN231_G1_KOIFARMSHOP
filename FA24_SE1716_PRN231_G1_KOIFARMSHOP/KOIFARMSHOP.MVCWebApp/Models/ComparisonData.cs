using KOIFARMSHOP.Data.Models;

namespace KOIFARMSHOP.MVCWebApp.Models
{
    public class ComparisonData
    {
        public List<Animal> KoiFishList { get; set; }
        public List<string> ComparisonMessage { get; set; }
        public List<ComparisonResult> ComparisonResults { get; set; } = new List<ComparisonResult>();
    }
}
