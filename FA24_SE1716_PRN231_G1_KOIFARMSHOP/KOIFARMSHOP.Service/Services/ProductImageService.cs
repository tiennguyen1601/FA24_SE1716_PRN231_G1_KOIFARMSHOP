using KOIFARMSHOP.Data;
using KOIFARMSHOP.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOIFARMSHOP.Service.Services
{
    public interface IProductImageService
    {
        Task SaveProductImage(int productId, List<string>? images);
    }

    public class ProductImageService : IProductImageService
    {
        private readonly UnitOfWork _unitOfWork;
        public ProductImageService()
        {
            _unitOfWork ??= new UnitOfWork();
        }
        public async Task SaveProductImage(int productId, List<string>? images)
        {
            var currProduct = await _unitOfWork.ProductRepository.GetByIdAsync(productId);
            if (currProduct == null) return;

            var currProductImages = await _unitOfWork.ProductImageRepository.GetProductImagesByProductId(currProduct.ProductId);

            foreach (var item in currProductImages)
            {
                await _unitOfWork.ProductImageRepository.RemoveAsync(item);
            }

            foreach (var image in images)
            {
                ProductImage newProductImage = new ProductImage
                {
                    ProductId = currProduct.ProductId,
                    ImageUrl = image
                };

                await _unitOfWork.ProductImageRepository.CreateAsync(newProductImage);
            }
        }
    }
}
