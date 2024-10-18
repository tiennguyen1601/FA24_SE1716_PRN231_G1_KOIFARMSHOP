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
    public class ConsignmentRepository : GenericRepository<Consignment>
    {
        public ConsignmentRepository() { }
        public ConsignmentRepository(FA24_SE1716_PRN231_G1_KOIFARMSHOPContext context) => _context = context;

        public async Task<Consignment> GetByIdDetail(int id) 
        {
            var consignment = await _context.Consignments
            .Include(c => c.Animal)
          
            .Include(c => c.Customer)  
            .Include(c => c.Order)  
            .FirstOrDefaultAsync(c => c.ConsignmentId == id);

            return consignment;
        }

        public async Task<List<Consignment>> GetAllDetail(int customerId)
        {
            var consignments = await _context.Consignments
                .Include(c => c.Animal)
                .Include(c => c.Customer)
                //.Include(c => c.Order)
                .Where(c => c.CustomerId == customerId)  
                .ToListAsync();

            return consignments;
        }


        public async Task<bool> Delete(Consignment consignment)
        {
            var currConsignment = await _context.Consignments.FirstOrDefaultAsync(x => x.ConsignmentId == consignment.ConsignmentId);

            currConsignment.Status = "Deleted";

            _context.Update(currConsignment);

            await _context.SaveChangesAsync();
            return true;
        }

    }
}
