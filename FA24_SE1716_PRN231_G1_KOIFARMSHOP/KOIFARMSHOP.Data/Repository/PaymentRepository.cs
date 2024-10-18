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
    public class PaymentRepository : GenericRepository<Payment>
    {
        public PaymentRepository() { }
        public PaymentRepository(FA24_SE1716_PRN231_G1_KOIFARMSHOPContext context) => _context = context;

        public List<Payment> GetAll()
        {
            return _context.Set<Payment>()
                .Include(p => p.Customer)
                .Include(p => p.Order)
                .ToList();
        }

        public async Task<List<Payment>> GetAllAsync()
        {
            return await _context.Set<Payment>()
                .Include(p => p.Customer)
                .Include(p => p.Order)
                .ToListAsync();
        }

        public Payment GetById(int id)
        {
            return _context.Set<Payment>()
                .Include(p => p.Customer)
                .Include(p => p.Order)
                .FirstOrDefault(p => p.PaymentId == id);
        }

        public async Task<Payment> GetByIdAsync(int id)
        {
            return await _context.Set<Payment>()
                .Include(p => p.Customer)
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.PaymentId == id);
        }
    }
}
