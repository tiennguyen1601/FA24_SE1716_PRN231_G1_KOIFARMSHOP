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
            var animal = await _animalService.GetByID(id);

            return animal;
        }

        // PUT: api/Animals/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IBusinessResult> PutAnimal(Animal animal)
        {
           return await _animalService.Save(animal);
        }

        // POST: api/Animals
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IBusinessResult> PostAnimal(Animal animal)
        {
            return await _animalService.Save(animal);

        }

        // DELETE: api/Animals/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnimal(int id)
        {
            var animal = await _animalService.GetByID(id);
            if (animal == null)
            {
                return NotFound();
            }
            await _animalService.DeleteByID(id);

            return NoContent();
        }
        
    }
}
