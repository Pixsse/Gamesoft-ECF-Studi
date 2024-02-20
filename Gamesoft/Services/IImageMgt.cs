using Gamesoft.Models;

namespace Gamesoft.Services
{
    public interface IImageMgt
    {
        public void UploadAndInsertImages(int productId, List<IFormFile> imageFiles);
        public bool InsertImage(int productId, string imagePath);
    }
}
