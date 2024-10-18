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
    public class AnimalRepository : GenericRepository<Animal>
    {
        public AnimalRepository() { }
        public AnimalRepository(FA24_SE1716_PRN231_G1_KOIFARMSHOPContext context) => _context = context;

        public async Task<List<Animal>> GetAnimals(int? page, int? size)
        {
            var pageIndex = (page.HasValue && page > 0) ? page.Value : 1;
            var sizeIndex = (size.HasValue && size > 0) ? size.Value : 10;

            return await _context.Animals
                .Include(x => x.CreatedByNavigation)
                .Include(x => x.ModifiedByNavigation)
                .Skip((pageIndex - 1) * sizeIndex)
                .Take(sizeIndex)
                .ToListAsync();
        }

        public async Task<List<Animal>> GetAnimals()
        {
            return await _context.Animals
                .Include(x => x.CreatedByNavigation)
                .Include(x => x.ModifiedByNavigation)
                .ToListAsync();
        }
    }
}
