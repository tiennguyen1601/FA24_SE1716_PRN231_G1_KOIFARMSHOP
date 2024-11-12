using KOIFARMSHOP.Data.Base;
using KOIFARMSHOP.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOIFARMSHOP.Data.Repository
{
    public class AttributeRepository : GenericRepository<Models.Attribute>
    {
        public AttributeRepository() { }
        public AttributeRepository(FA24_SE1716_PRN231_G1_KOIFARMSHOPContext context) => _context = context;


    }
}
