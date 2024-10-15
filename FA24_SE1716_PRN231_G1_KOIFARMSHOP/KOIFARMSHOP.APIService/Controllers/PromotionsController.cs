using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KOIFARMSHOP.Data.Models;
using KOIFARMSHOP.Service.Services;
using KOIFARMSHOP.Service.Base;

namespace KOIFARMSHOP.APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionsController : ControllerBase
    {
        //private readonly FA24_SE1716_PRN231_G1_KOIFARMSHOPContext _context;
        private readonly IPromotionService _promotionService;

        public PromotionsController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        // GET: api/Promotions
        [HttpGet]
        public async Task<IBusinessResult> GetPromotions()
        {
            return await _promotionService.GetAll();
        }

        // GET: api/Promotions/5
        [HttpGet("{id}")]
        public async Task<IBusinessResult> GetPromotion(int id)
        {
            var promotion = await _promotionService.GetByID(id);

            return promotion;
        }

        // PUT: api/Promotions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IBusinessResult> PutPromotion( Promotion promotion)
        {
            return await _promotionService.Save(promotion);
        }

        // POST: api/Promotions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IBusinessResult> PostPromotion(Promotion promotion)
        {
            return await _promotionService.Save(promotion);
        }

        // DELETE: api/Promotions/5
        //[HttpDelete("{id}")]
        //public async Task<IBusinessResult> DeletePromotion(int id)
        //{
        //    var animal = await _promotionService.GetByID(id);
        //    if (animal == null)
        //    {
        //        return NotFound();
        //    }
        //    await _promotionService.DeleteByID(id);

        //    return NoContent();
        //}

        
    }
}
