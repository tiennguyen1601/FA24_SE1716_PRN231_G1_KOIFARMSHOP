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
    public class CategoryRepository : GenericRepository<Category>
    {
        public CategoryRepository() { }
        public CategoryRepository(FA24_SE1716_PRN231_G1_KOIFARMSHOPContext context) => _context = context;

        public async Task<bool> RemoveCategory(Category category)
        {
            var products = await _context.Products.Where(x => x.CategoryId == category.CategoryId).ToListAsync();

            if (products.Count > 0)
            {
                return false;
            }
            else
            {
                category.Status = "Inactive";
                _context.Update(category);
                await _context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> ActivateDeactivate(Category category)
        {
            category.Status = category.Status.Equals(StatusEnums.Active.ToString()) ? StatusEnums.Inactive.ToString() : StatusEnums.Active.ToString();
            _context.Update(category);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
