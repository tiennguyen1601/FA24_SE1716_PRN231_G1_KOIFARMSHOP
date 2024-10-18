using KOIFARMSHOP.Service.Base;
using KOIFARMSHOP.Data;
using KOIFARMSHOP.Common;
using KOIFARMSHOP.Data.Models;
using System.Collections.Generic;
using System.Linq.Expressions;

using KOIFARMSHOP.Service.Services.JWTService;

using KOIFARMSHOP.Data.DTO.AniamlDTO;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace KOIFARMSHOP.Service.Services
{
    public interface IAnimalService
    {
        Task<IBusinessResult> GetAll();
        Task<IBusinessResult> GetByID(int id);


        Task<IBusinessResult> GetAllByUser(string token);

        Task<IBusinessResult> Save(AnimalReqModel request, int? animalId = null);

        Task<IBusinessResult> DeleteByID(int id);
        Task<IBusinessResult> CompareMultipleKoiFishPrices(List<int> koiFishIds);


    }
    public class AnimalService : IAnimalService
    {
        private readonly UnitOfWork _unitOfWork;

        private readonly IJWTService _jwtService;

        private readonly IMapper _mapper;
        public AnimalService(UnitOfWork unitOfWork, IMapper mapper, IJWTService jWTService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jwtService = jWTService;
        }

        public async Task<IBusinessResult> GetAll()
        {
            var queryableList = await _unitOfWork.AnimalRepository.GetAllAsync();

            var list = await queryableList
                               .Include(a => a.CreatedByNavigation)
                               .Include(a => a.ModifiedByNavigation)
                               .ToListAsync();

            if (list == null || !list.Any())
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<Animal>());
            }

            return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, list);
        }

        public async Task<IBusinessResult> GetByID(int id)
        {
            #region Business rule
            #endregion
            var list = await _unitOfWork.AnimalRepository.GetByIdAsync(id);
            if (list == null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<Animal>());
            }
            else
            {
                return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, list);
            }

        }


        public async Task<IBusinessResult> GetAllByUser(string token)
        {

            var userIdString = _jwtService.decodeToken(token, "userid");
            if (!int.TryParse(userIdString, out int userId))
            {
                return new BusinessResult(Const.FAIL_CREATE_CODE, "Invalid user ID.", null);
            }
            var list = await _unitOfWork.AnimalRepository.GetAllByUserId(userId);
            if (list == null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<Animal>());
            }
            else
            {
                return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, list);
            }
        }


        public async Task<IBusinessResult> Save(AnimalReqModel request, int? animalId = null)

        {
            try
            {
                Animal animal;

                if (animalId.HasValue)
                {
                    animal = await _unitOfWork.AnimalRepository.GetByIdAsync(animalId.Value);
                    if (animal == null)
                    {
                        return new BusinessResult(Const.WARNING_NO_DATA_CODE, "Animal not found.");
                    }

                    _mapper.Map(request, animal);
                }
                else
                {
                    animal = _mapper.Map<Animal>(request);
                    animal.CreatedAt = DateTime.Now;
                    animal.CreatedBy = request.CreatedBy;
                }

                if (request.AnimalImages != null)
                {
                    animal.AnimalImages = request.AnimalImages
                        .Select(url => new AnimalImage { ImageUrl = url })
                        .ToList();
                }

                int result = animalId.HasValue
                    ? await _unitOfWork.AnimalRepository.UpdateAsync(animal)
                    : await _unitOfWork.AnimalRepository.CreateAsync(animal);

                if (result > 0)
                {
                    return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, animal);
                }

                return new BusinessResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, animal);
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }

        public async Task<IBusinessResult> DeleteByID(int id)
        {
            try
            {
                var animalById = await _unitOfWork.AnimalRepository.GetByIdAsync(id);
                if (animalById == null)
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new Animal());
                }
                else
                {
                    var result = await _unitOfWork.AnimalRepository.RemoveAsync(animalById);
                    if (result)
                    {
                        return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, animalById);
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, animalById);
                    }
                }
            }
            catch (Exception ex) { return new BusinessResult(Const.ERROR_EXCEPTION, ex.ToString()); }
        }

        public async Task<IBusinessResult> CompareMultipleKoiFishPrices(List<int> koiFishIds)
        {
            try
            {
                // Fetch koi fish details for all provided IDs
                var koiFishList = new List<Animal>();
                var validKoiFishIds = new List<int>();
                foreach (var koiFishId in koiFishIds)
                {
                    var koiFish = await _unitOfWork.AnimalRepository.GetByIdAsync(koiFishId);
                    if (koiFish != null)
                    {
                        koiFishList.Add(koiFish);
                        validKoiFishIds.Add(koiFishId); // Add to valid ID list
                    }
                }

                // Check if we have any valid koi fish
                if (koiFishList.Count == 0)
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE,
                        "No valid koi fish found for the given IDs.",
                        koiFishList);
                }

                // Check if we have enough data to compare
                if (koiFishList.Count < 2)
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE,
                        "At least two koi fish are required for comparison.",
                        koiFishList);
                }

                // Sort the koi fish by price in ascending order
                koiFishList = koiFishList.OrderBy(k => k.Price).ToList();

                // Create a comparison message
                List<string> comparisonMessage = new List<string>();
                comparisonMessage.Add("Comparison of Koi Fish Prices:");
                for (int i = 0; i < koiFishList.Count - 1; i++)
                {
                    var currentKoiFish = koiFishList[i];
                    var nextKoiFish = koiFishList[i + 1];
                    if (currentKoiFish.Price < nextKoiFish.Price)
                    {
                        comparisonMessage.Add($"Koi fish with ID {nextKoiFish.AnimalId} ({nextKoiFish.Species}) is more expensive than koi fish with ID {currentKoiFish.AnimalId} ({currentKoiFish.Species})");
                    }
                    else if (currentKoiFish.Price == nextKoiFish.Price)
                    {
                        comparisonMessage.Add($"Koi fish with ID {nextKoiFish.AnimalId} ({nextKoiFish.Species}) has the same price as koi fish with ID {currentKoiFish.AnimalId} ({currentKoiFish.Species}).");
                    }
                }

                // Return the list of koi fish with comparison message
                var result = new
                {
                    KoiFishList = koiFishList,
                    ComparisonMessage = comparisonMessage
                };
                return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, result);
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }


    }
}
