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
    public class OrderDetailRepository : GenericRepository<OrderDetail>
    {
        public OrderDetailRepository() { }
        public OrderDetailRepository(FA24_SE1716_PRN231_G1_KOIFARMSHOPContext context) => _context = context;
    }
}
