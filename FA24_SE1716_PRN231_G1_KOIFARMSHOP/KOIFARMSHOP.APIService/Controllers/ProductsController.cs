using KOIFARMSHOP.Data.DTO.ProductDTO;
using KOIFARMSHOP.Data.Models;
using KOIFARMSHOP.Service.Base;
using KOIFARMSHOP.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        [Authorize]
        public async Task<IBusinessResult> GetProducts()
        {
            return await _productService.GetAll();
        }

        [HttpGet("{productId}")]
        public async Task<IBusinessResult> GetProduct(int productId)
        {
            return await _productService.GetById(productId);
        }

        // PUT: api/Products/5
        [HttpPut]
        public async Task<IBusinessResult> PutProduct(Product product)
        {

            return await _productService.Save(product);
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
            return await _productService.Save(newProduct);
        }


        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IBusinessResult> DeleteProduct(int id)
        {
            return await _productService.DeleteById(id);
        }
    }
}
