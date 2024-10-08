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
    public class OrderRepository : GenericRepository<Order>
    {
        public OrderRepository() { }
        public OrderRepository(FA24_SE1716_PRN231_G1_KOIFARMSHOPContext context) => _context = context;


        public async Task<Order> GetByIdDetail(int id)
        {
            var order = await _context.Orders
            .Include(c => c.OrderDetails)
            .FirstOrDefaultAsync(c => c.OrderId == id);

            return order;
        }

        public async Task<List<Order>> GetAllDetail()
        {
            var order = await _context.Orders
            .Include(c => c.OrderDetails)
                .ToListAsync();

            return order;
        }
    }
}
