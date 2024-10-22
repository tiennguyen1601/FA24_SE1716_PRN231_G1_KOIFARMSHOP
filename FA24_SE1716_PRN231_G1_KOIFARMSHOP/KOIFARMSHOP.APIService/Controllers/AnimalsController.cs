using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KOIFARMSHOP.Data.Models;
using KOIFARMSHOP.Service.Services;
using KOIFARMSHOP.Service.Base;
using KOIFARMSHOP.Common;
using KOIFARMSHOP.Data.DTO.AniamlDTO;

namespace KOIFARMSHOP.APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        private readonly IAnimalService _animalService;

        public AnimalsController(IAnimalService animalService)
        {
            _animalService = animalService;
        }

        // GET: api/Animals
        [HttpGet]
        public async Task<IBusinessResult> GetAnimals(int? page = 1, int? size = 10)
        {
            return await _animalService.GetAll(page, size);
        }
        
        [HttpGet]
        [Route("get-Animal-by-user")]
        public async Task<IBusinessResult> GetAnimal123()
        {
            return await _animalService.GetAll();
        }

        // GET: api/Animals/5
        [HttpGet("{id}")]
        public async Task<IBusinessResult> GetAnimal(int id)
        {
            return await _animalService.GetByID(id);
        }

        // GET: api/Animals/User
        [HttpGet("User")]
        public async Task<IBusinessResult> GetAnimalsByUser()
        {
            string token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var animal = await _animalService.GetAllByUser(token);
            return animal;
        }

        // GET: api/Animals/search
        [HttpGet("search")]
        public async Task<IBusinessResult> SearchAnimals([FromQuery] AnimalFilterReqModel? filterModel, [FromQuery] string? searchValue, int? page = 1, int? size = 10)
        {
            return await _animalService.SearchAnimals(filterModel, searchValue, page, size);
        }

        // PUT: api/Animals/5
        [HttpPut("{id}")]
        public async Task<IBusinessResult> PutAnimal(int id, [FromBody] AnimalReqModel request)
        {
            string token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            if (!ModelState.IsValid)
            {
                return new BusinessResult(Const.FAIL_CREATE_CODE, "Invalid model state");
            }

            return await _animalService.Save(token, request, id);
        }

        // POST: api/Animals
        [HttpPost]
        public async Task<IBusinessResult> PostAnimal([FromBody] AnimalReqModel request)
        {
            string token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            if (!ModelState.IsValid)
            {
                return new BusinessResult(Const.FAIL_CREATE_CODE, "Invalid model state");
            }

            return await _animalService.Save(token, request);
        }

        // DELETE: api/Animals/5
        [HttpDelete("{id}")]
        public async Task<IBusinessResult> DeleteAnimal(int id)
        {
            return await _animalService.DeleteByID(id);
        }

        // POST: api/Animals/CompareMultipleFish
        [HttpPost("CompareMultipleFish")]
        public async Task<IBusinessResult> CompareMultipleAnimal([FromBody] CompareMultipleAnimalRequestModels request)
        {
            var koiFishList = new List<Animal>();

            foreach (var id in request.Ids)
            {
                var result = await _animalService.GetByID(id);

                if (result == null)
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, $"Koi fish with ID {id} was not found.");
                }

                var fish = result.Data as Animal;

                if (fish == null)
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, $"Koi fish with ID {id} was not found.");
                }

                koiFishList.Add(fish);
            }

            var koiFishIds = koiFishList.Select(f => f.AnimalId).ToList();

            var comparisonResult = await _animalService.CompareMultipleKoiFishAttributes(koiFishIds, request.ComparisonAttributes);

            return comparisonResult;
        }

    }
}
