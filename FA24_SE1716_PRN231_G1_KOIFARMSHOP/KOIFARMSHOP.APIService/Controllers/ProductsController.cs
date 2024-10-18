using KOIFARMSHOP.Common;
using KOIFARMSHOP.Data.DTO.ProductDTO;
using KOIFARMSHOP.Data.Models;
using KOIFARMSHOP.Service.Base;
using KOIFARMSHOP.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using Org.BouncyCastle.Asn1.X509;

namespace KOIFARMSHOP.APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        //private readonly FA24_SE1716_PRN231_G1_KOIFARMSHOPContext _context;

        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IBusinessResult> GetProducts(int? page = 1, int? size = 10)
        {
            return await _productService.GetAll(page, size);
        }

        [HttpGet("{productId}")]
        public async Task<IBusinessResult> GetProduct(int productId)
        {
            return await _productService.GetById(productId);
        }

        [HttpGet]
        [Route("search")]
        public async Task<IBusinessResult> SearchProducts([FromQuery] ProductFilterReqModel? productFilterReqModel, string? searchValue, int? page = 1, int? size = 10)
        {
            return await _productService.SearchProducts(productFilterReqModel, searchValue, page, size);
        }

        [HttpGet]
        [Route("brand")]
        public async Task<IBusinessResult> GetBrandName()
        {
            return await _productService.GetBrandName();
        }

        // PUT: api/Products/5
        [HttpPut]
        public async Task<IBusinessResult> PutProduct(UpdateProductReqModel updateProduct)
        {

            var currProduct = await _productService.GetProductById(updateProduct.ProductId);

            if (currProduct == null) return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<Product>());

            currProduct.Name = !string.IsNullOrEmpty(updateProduct.Name) ? updateProduct.Name : currProduct.Name;
            currProduct.Description = !string.IsNullOrEmpty(updateProduct.Description) ? updateProduct.Description : currProduct.Description;
            currProduct.Price = updateProduct.Price != null ? (decimal)updateProduct.Price : currProduct.Price;
            currProduct.StockQuantity = updateProduct.StockQuantity != null ? (int)updateProduct.StockQuantity : currProduct.StockQuantity;
            currProduct.Brand = !string.IsNullOrEmpty(updateProduct.Brand) ? updateProduct.Brand : currProduct.Brand; ;
            currProduct.Weight = updateProduct.Weight != null ? (int)updateProduct.Weight : currProduct.Weight; ;
            currProduct.Discount = updateProduct.Discount != null ? (decimal)updateProduct.Discount : currProduct.Discount; ;
            currProduct.ExpiryDate = updateProduct.ExpiryDate.HasValue ? updateProduct.ExpiryDate.Value : currProduct.ExpiryDate;
            currProduct.ManufacturingDate = updateProduct.ManufacturingDate.HasValue ? updateProduct.ManufacturingDate.Value : currProduct.ManufacturingDate;
            currProduct.CategoryId = updateProduct.CategoryId != null ? updateProduct.CategoryId : currProduct.CategoryId;
            currProduct.UpdatedAt = DateTime.Now;

            //_context.Products.Update(currProduct);
            //await _context.SaveChangesAsync();

            var result = await _productService.Save(currProduct, updateProduct.Images);

            return result;
        }

        [HttpPost]
        public async Task<IBusinessResult> PostProduct(CreateProductReqModel product)
        {
            Product newProduct = new Product
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                Brand = product.Name,
                Weight = product.Weight,
                Discount = product.Discount,
                ExpiryDate = product.ExpiryDate,
                ManufacturingDate = product.ManufacturingDate,
                CategoryId = product.CategoryId,
                Status = "Active",
                CreatedAt = DateTime.Now,
                CreatedBy = product.CreatedBy
            };
            return await _productService.Save(newProduct, product.Images);
        }


        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IBusinessResult> DeleteProduct(int id)
        {
            return await _productService.DeleteById(id);
        }
    }
}
