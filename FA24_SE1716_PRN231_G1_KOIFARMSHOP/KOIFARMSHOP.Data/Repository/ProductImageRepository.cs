using KOIFARMSHOP.Data.Base;
using KOIFARMSHOP.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOIFARMSHOP.Data.Repository
{
    public class ProductImageRepository : GenericRepository<ProductImage>
    {
        public ProductImageRepository() { }
        public ProductImageRepository(FA24_SE1716_PRN231_G1_KOIFARMSHOPContext context) => _context = context;

        public async Task<List<ProductImage>> GetProductImagesByProductId(int productId)
        {
            return await _context.ProductImages.Where(x => x.ProductId == productId).ToListAsync();
        }
    }
}
