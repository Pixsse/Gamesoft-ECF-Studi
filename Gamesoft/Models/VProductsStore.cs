namespace Gamesoft.Models
{
    public partial class VProductsStore
    {
        public int ProductId { get; set; }

        public int? Score { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public string Image { get; set; }

        public int StatusId { get; set; }

        public int GenreId { get; set; }

        public DateOnly DateCreated { get; set; }

        public string Description { get; set; }

        public int SupportId { get; set; }

        public int EngineId { get; set; }
    }
}
