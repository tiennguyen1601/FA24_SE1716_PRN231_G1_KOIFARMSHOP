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

        public async Task<List<Animal>> GetAllByUserId(int userId)
        {
            var animals = await _context.Animals
                .Where(a => a.CreatedBy == userId)
                .ToListAsync();

            return animals;
        }

    }
}
