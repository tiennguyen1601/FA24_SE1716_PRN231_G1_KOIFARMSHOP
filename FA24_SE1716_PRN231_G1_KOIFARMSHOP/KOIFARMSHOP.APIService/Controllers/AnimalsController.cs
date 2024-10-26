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
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Org.BouncyCastle.Tls;
using System.Drawing;
using Microsoft.Extensions.Logging.Abstractions;

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

            var currAnimal = await _animalService.GetAnimalById(id);

            if (currAnimal == null) return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<Animal>());

            var rawImages = request.Images[0];

            List<string> Images = new List<string>(rawImages.Split(new[] { ", " }, StringSplitOptions.None));

            if (!ModelState.IsValid)
            {
                return new BusinessResult(Const.FAIL_CREATE_CODE, "Invalid model state");
            }

            currAnimal.Name = !string.IsNullOrEmpty(request.Name) ? request.Name : currAnimal.Name;
            currAnimal.Origin = !string.IsNullOrEmpty(request.Origin) ? request.Origin : currAnimal.Origin;
            currAnimal.Species = !string.IsNullOrEmpty(request.Species) ? request.Species : currAnimal.Species;
            currAnimal.Type = !string.IsNullOrEmpty(request.Type) ? request.Type : currAnimal.Type;
            currAnimal.Gender = !string.IsNullOrEmpty(request.Gender) ? request.Gender : currAnimal.Gender;
            currAnimal.Size = !string.IsNullOrEmpty(request.Size) ? request.Size : currAnimal.Size;
            currAnimal.Certificate = !string.IsNullOrEmpty(request.Certificate) ? request.Certificate : currAnimal.Certificate;
            currAnimal.Price = request.Price != null ? request.Price : currAnimal.Price;
            currAnimal.Status = !string.IsNullOrEmpty(request.Status) ? request.Status : currAnimal.Status;
            currAnimal.MaintenanceCost = request.MaintenanceCost != null ? request.MaintenanceCost : currAnimal.MaintenanceCost;
            currAnimal.Color = !string.IsNullOrEmpty(request.Color) ? request.Color : currAnimal.Color;
            currAnimal.AmountFeed = request.AmountFeed != null ? request.AmountFeed : currAnimal.AmountFeed;
            currAnimal.HealthStatus = !string.IsNullOrEmpty(request.HealthStatus) ? request.HealthStatus : currAnimal.HealthStatus;
            currAnimal.FarmOrigin = !string.IsNullOrEmpty(request.FarmOrigin) ? request.FarmOrigin : currAnimal.FarmOrigin;
            currAnimal.BirthYear = request.BirthYear != null ? request.BirthYear : currAnimal.BirthYear;
            currAnimal.Description = !string.IsNullOrEmpty(request.Description) ? request.Description : currAnimal.Description;
            currAnimal.UpdatedAt = DateTime.Now;

            return await _animalService.Save(currAnimal, Images, token, id);
        }

        // POST: api/Animals
        [HttpPost]
        public async Task<IBusinessResult> PostAnimal([FromBody] AnimalReqModel request)
        {
            string token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            var rawImages = request.Images[0];
            List<string> Images = new List<string>(rawImages.Split(new[] { ", " }, StringSplitOptions.None));

            Animal newAnimal = new Animal
            {
                Name = request.Name,
                Origin = request.Origin,
                Species = request.Species,
                Type = request.Type,
                Gender = request.Gender,
                Size = request.Size,
                Certificate = request.Certificate,
                Price = request.Price,
                Status = request.Status,
                MaintenanceCost = request.MaintenanceCost,
                Color = request.Color,
                AmountFeed = request.AmountFeed,
                HealthStatus = request.HealthStatus,
                FarmOrigin = request.FarmOrigin,
                BirthYear = request.BirthYear,
                Description = request.Description,
                CreatedAt = DateTime.Now
            };
            return await _animalService.Save(newAnimal, Images, token);
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
