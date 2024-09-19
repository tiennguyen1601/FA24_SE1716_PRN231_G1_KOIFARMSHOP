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
    public class PromotionRepository : GenericRepository<Promotion>
    {
        public PromotionRepository() { }
        public PromotionRepository(FA24_SE1716_PRN231_G1_KOIFARMSHOPContext context) => _context = context;
    }
}
