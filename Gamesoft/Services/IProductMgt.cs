using Gamesoft.Models;

namespace Gamesoft.Services
{
    public interface IProductMgt
    {
        public int CreateProduct(Product product);
        public bool UpdateProduct(Product product);
        public void UploadAndInsertImages(int productId, List<IFormFile> imageFiles, IFormFile featuredImage);
        public List<ProductImage> GetImages(int productId);
        public List<VProductsStore> GetProductsStore(IQueryCollection query);
        public bool ExistFavorite(int accountId, int productId);
        public bool AddToFavorite(int accountId, int productId);
        public bool DeleteFavorite(int accountId, int productId);
        public int CountFavorite(int productId);
        public List<ProductFavorite> GetProductFavorites(int accountId);
        public List<VProductsStore> GetProductStoreFavorites(int accountId);
        public ProductImage GetLastProductImage();
    }
}
