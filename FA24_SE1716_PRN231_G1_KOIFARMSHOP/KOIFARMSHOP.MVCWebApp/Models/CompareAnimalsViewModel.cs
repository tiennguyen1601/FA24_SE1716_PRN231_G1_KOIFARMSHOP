

using KOIFARMSHOP.Data.Models;

namespace KOIFARMSHOP.MVCWebApp.Models
{
    public class CompareAnimalsViewModel
    {
        public List<Animal> Animals { get; set; } = new List<Animal>(); 
        public List<int> SelectedAnimalIds { get; set; } = new List<int>(); 
        public List<string> SelectedAttributes { get; set; } = new List<string>();
        public List<ComparisonResult> ComparisonResults { get; set; } = new List<ComparisonResult>(); 
        public List<string> ComparisonMessages { get; set; } = new List<string>(); 
    }

    public class ComparisonResult
    {
        public string AttributeName { get; set; } 
        public Dictionary<int, string> Values { get; set; } = new Dictionary<int, string>(); 
    }
}