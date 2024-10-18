using KOIFARMSHOP.Data.Base;
using KOIFARMSHOP.Data.Enums;
using KOIFARMSHOP.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOIFARMSHOP.Data.Repository
{
    public class ProductRepository : GenericRepository<Product>
    {
        public ProductRepository() { }
        public ProductRepository(FA24_SE1716_PRN231_G1_KOIFARMSHOPContext context) => _context = context;

        public async Task<List<Product>> GetProducts(int? page, int? size)
        {
            var pageIndex = (page.HasValue && page > 0) ? page.Value : 1;
            var sizeIndex = (size.HasValue && size > 0) ? size.Value : 10;


            return await _context.Products.Include(x => x.Category)
                .Include(x => x.CreatedByNavigation)
                .Include(x => x.ModifiedByNavigation)
                .Skip((pageIndex - 1) * sizeIndex)
                .Take(sizeIndex)
                .ToListAsync();
        }

        public async Task<List<Product>> GetProducts()
        {
            return await _context.Products.Include(x => x.Category)
                .Include(x => x.CreatedByNavigation)
                .Include(x => x.ModifiedByNavigation)
                .Include(x => x.Category)
                .ToListAsync();
        }

        public async Task<Product> GetProductById(int productId)
        {
            return await _context.Products.Include(x => x.Category)
                .Include(x => x.CreatedByNavigation)
                .Include(x => x.ModifiedByNavigation)
                .Include(x => x.Category)
                .Include(x => x.ProductImages)
                .FirstOrDefaultAsync(x => x.ProductId == productId);
        }

        public async Task<bool> Delete(Product product)
        {
            var currProduct = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == product.ProductId);

            currProduct.Status = "Inactive";

            _context.Update(currProduct);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActivateDeactivate(Product product)
        {
            var currProduct = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == product.ProductId);

            currProduct.Status =  currProduct.Status.Equals(StatusEnums.Active.ToString()) ? StatusEnums.Inactive.ToString() : StatusEnums.Active.ToString();

            _context.Update(currProduct);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
