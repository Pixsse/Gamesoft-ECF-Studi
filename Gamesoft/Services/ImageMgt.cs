using Gamesoft.Contexts;
using Gamesoft.Models;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Gamesoft.Services
{
    public class ImageMgt : IImageMgt
    {
        private readonly Contexts.GamesoftContext _context;

        public ImageMgt(Contexts.GamesoftContext context)
        {
            _context = context;
        }

        public async void UploadAndInsertImages(int productId, List<IFormFile> imageFiles)
        {
            foreach (var imageFile in imageFiles)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    var baseDirectory = Path.Combine("wwwroot/img/products", productId.ToString()) ;
                    var imageUrl = Path.Combine(baseDirectory, imageFile.FileName);

                    var fileDirectory = Path.Combine(Directory.GetCurrentDirectory(), baseDirectory);
                    Directory.CreateDirectory(fileDirectory);

                    var filePath = Path.Combine(fileDirectory, imageFile.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                        InsertImage(productId, imageUrl);
                    }
                }
            }
        }

        public bool InsertImage(int productId, string imagePath)
        {
            if (productId == 0)
            {
                return false;
            }
            ProductImage productImage = new ProductImage();
            productImage.ProductId = productId;
            productImage.Image = imagePath;

            _context.Add(productImage);
            _context.SaveChanges();

            return true;
        }
    }
}
