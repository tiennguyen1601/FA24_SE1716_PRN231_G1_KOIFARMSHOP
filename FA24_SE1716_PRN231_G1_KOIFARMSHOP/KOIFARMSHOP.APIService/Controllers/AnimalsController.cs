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
using KOIFARMSHOP.Common;
using Newtonsoft.Json;
using KOIFARMSHOP.Data.DTO.AniamlDTO;

namespace KOIFARMSHOP.APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        //private readonly FA24_SE1716_PRN231_G1_KOIFARMSHOPContext _context;
        private readonly IAnimalService _animalService;

        public AnimalsController(IAnimalService animalService)
        {
            _animalService = animalService;
        }


        // GET: api/Animals
        [HttpGet]
        public async Task<IBusinessResult> GetAnimals()
        {
            return await _animalService.GetAll();
        }

        // GET: api/Animals/5
        [HttpGet("{id}")]
        public async Task<IBusinessResult> GetAnimal(int id)
        {
            return await _animalService.GetByID(id);
        }

        [HttpGet("User")]
        public async Task<IBusinessResult> GetAnimalsByUser()
        {
            string token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var animal = await _animalService.GetAllByUser(token);
            return animal;
        }

        // PUT: api/Animals/5
        [HttpPut("{id}")]
        public async Task<IBusinessResult> PutAnimal(int id, [FromBody] AnimalReqModel request)
        {
            if (!ModelState.IsValid)
            {
                return new BusinessResult(Const.FAIL_CREATE_CODE, "Invalid model state");
            }

            return await _animalService.Save(request, id);
        }

        // POST: api/Animals
        [HttpPost]
        public async Task<IBusinessResult> PostAnimal([FromBody] AnimalReqModel request)
        {
            if (!ModelState.IsValid)
            {
                return new BusinessResult(Const.FAIL_CREATE_CODE, "Invalid model state");
            }

            return await _animalService.Save(request);
        }

        // DELETE: api/Animals/5
        [HttpDelete("{id}")]
        public async Task<IBusinessResult> DeleteAnimal(int id)
        {
            return await _animalService.DeleteByID(id);
        }

        [HttpPost("CompareMultipleFish")]
        public async Task<IActionResult> CompareMultipleAnimal(List<int> ids)
        {
            var koiFishList = new List<Animal>();

            foreach (var id in ids)
            {
                var result = await _animalService.GetByID(id);

                if (result == null )
                {
                    return NotFound($"Koi fish with ID {id} was not found.");
                }

                var fish = result.Data as Animal;

                if (fish == null)
                {
                    return NotFound($"Koi fish with ID {id} was not found.");
                }

                koiFishList.Add(fish);
            }

            var koiFishIds = koiFishList.Select(f => f.AnimalId).ToList();

            var comparisonResult = await _animalService.CompareMultipleKoiFishPrices(koiFishIds);

            return Ok(comparisonResult);
        }



    }
}
