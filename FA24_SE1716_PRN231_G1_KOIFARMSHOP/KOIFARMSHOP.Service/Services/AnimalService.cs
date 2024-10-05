using KOIFARMSHOP.Service.Base;
using KOIFARMSHOP.Data;
using KOIFARMSHOP.Common;
using KOIFARMSHOP.Data.Models;
using System.Collections.Generic;
using System.Linq.Expressions;
namespace KOIFARMSHOP.Service.Services
{
    public interface IAnimalService
    {
        Task<IBusinessResult> GetAll();
        Task<IBusinessResult> GetByID(int id);
        Task<IBusinessResult> Save(Animal animal);
        Task<IBusinessResult> DeleteByID(int id);
        Task<IBusinessResult> CompareMultipleKoiFishPrices(List<int> koiFishIds);


    }
    public class AnimalService : IAnimalService
    {
        private readonly UnitOfWork _unitOfWork;
        public AnimalService()
        {
            _unitOfWork ??= new UnitOfWork();
        }
        public async Task<IBusinessResult> GetAll()
        {
            var list = await _unitOfWork.AnimalRepository.GetAllAsync();
            if (list == null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<Animal>());
            }
            else
            {
                return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, list);
            }
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
        public async Task<IBusinessResult> Save(Animal animal)
        {
            try
            {
                int result = -1;
                var animalTmp = _unitOfWork.AnimalRepository.GetByIdAsync(animal.AnimalId);
                if (animalTmp != null)
                {
                    #region Business Rule
                    #endregion Business Rule

                    result = await _unitOfWork.AnimalRepository.UpdateAsync(animal);

                    if (result > 0)
                    {
                        return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<Animal>());
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, animal);
                    }
                }
                else
                {
                    result = await _unitOfWork.AnimalRepository.CreateAsync(animal);

                    if (result > 0)
                    {
                        return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, new List<Animal>());
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, animal);
                    }
                }
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
