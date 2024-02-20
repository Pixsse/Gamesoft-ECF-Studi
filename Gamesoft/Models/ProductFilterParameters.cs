namespace Gamesoft.Models
{
    public class ProductFilterParameters
    {        
        public string SearchedText { get; set; }
        public IList<int> Genre { get; set; }
        public IList<int> Status { get; set; }
        public DateOnly CreatedDate { get; set; }
    }
}
