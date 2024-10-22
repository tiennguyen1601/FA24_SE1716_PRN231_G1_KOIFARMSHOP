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
using Newtonsoft.Json.Linq;

namespace KOIFARMSHOP.Service.Services
{
    public interface IAnimalService
    {
        Task<IBusinessResult> GetAll();
        Task<IBusinessResult> GetByID(int id);
        Task<IBusinessResult> GetAllByUser(string token);

        Task<IBusinessResult> Save(string token, AnimalReqModel request, int? animalId = null);

        Task<IBusinessResult> DeleteByID(int id);
        Task<IBusinessResult> CompareMultipleKoiFishAttributes(List<int> koiFishIds, List<string> comparisonAttributes);

        Task<IBusinessResult> GetAll(int? page, int? size);
        Task<IBusinessResult> SearchAnimals(AnimalFilterReqModel? filterReqModel, string? searchValue, int? page, int? size);
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
            var queryableList = await _unitOfWork.AnimalRepository.GetAllAsync();

            var animal = await queryableList
                                .Include(a => a.CreatedByNavigation)
                                .Include(a => a.ModifiedByNavigation)
                                .FirstOrDefaultAsync(a => a.AnimalId == id);

            if (animal == null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, null);
            }
            else
            {
                return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, animal);
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


        public async Task<IBusinessResult> Save(string token, AnimalReqModel request, int? animalId = null)
        {
            var userIdString = _jwtService.decodeToken(token, "userid");
            if (!int.TryParse(userIdString, out int userId))
            {
                return new BusinessResult(Const.FAIL_CREATE_CODE, "Invalid user ID.", null);
            }
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
                    animal.CreatedBy = userId;
                    animal.ModifiedBy = userId;
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

        public async Task<IBusinessResult> GetAll(int? page, int? size)
        {
            var queryableAnimals = await _unitOfWork.AnimalRepository.GetAnimals();
            var totalItemCount = queryableAnimals.Count();

            var pagedAnimals = queryableAnimals
                .Skip(((page ?? 1) - 1) * (size ?? 10))
                .Take(size ?? 10)
                .ToList();

            var result = new Pagination<Animal>
            {
                TotalItems = totalItemCount,
                PageSize = size ?? 10,
                CurrentPage = page ?? 1,
                Data = pagedAnimals
            };

            return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
        }

        public async Task<IBusinessResult> SearchAnimals(AnimalFilterReqModel? filterReqModel, string? searchValue, int? page, int? size)
        {
            var allAnimals = await _unitOfWork.AnimalRepository.GetAllAsync();

            IQueryable<Animal> animalsQuery = allAnimals;

            if (!string.IsNullOrEmpty(searchValue))
            {
                animalsQuery = animalsQuery.Where(a => a.Name.Contains(searchValue) || a.Species.Contains(searchValue));
            }

            if (filterReqModel != null)
            {
                if (filterReqModel.Species != null && filterReqModel.Species.Any())
                {
                    animalsQuery = animalsQuery.Where(a => filterReqModel.Species.Contains(a.Species));
                }

                if (filterReqModel.Status != null && filterReqModel.Status.Any())
                {
                    animalsQuery = animalsQuery.Where(a => filterReqModel.Status.Contains(a.Status));
                }

                if (filterReqModel.MinPrice.HasValue)
                {
                    animalsQuery = animalsQuery.Where(a => a.Price >= filterReqModel.MinPrice.Value);
                }

                if (filterReqModel.MaxPrice.HasValue)
                {
                    animalsQuery = animalsQuery.Where(a => a.Price <= filterReqModel.MaxPrice.Value);
                }
            }

            var totalItemCount = await animalsQuery.CountAsync();

            var pagedAnimals = await animalsQuery
                .Skip(((page ?? 1) - 1) * (size ?? 10))
                .Take(size ?? 10)
                .ToListAsync();

            var result = new Pagination<Animal>
            {
                TotalItems = totalItemCount,
                PageSize = size ?? 10,
                CurrentPage = page ?? 1,
                Data = pagedAnimals
            };

            return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
        }

        public async Task<IBusinessResult> CompareMultipleKoiFishAttributes(List<int> koiFishIds, List<string> comparisonAttributes)
        {
            try
            {            
                var koiFishList = new List<Animal>();
                foreach (var koiFishId in koiFishIds)
                {
                    var koiFish = await _unitOfWork.AnimalRepository.GetByIdAsync(koiFishId);
                    if (koiFish != null)
                    {
                        koiFishList.Add(koiFish); 
                    }
                }

                if (koiFishList.Count < 2)
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, "Cần ít nhất hai cá koi để so sánh.");
                }

                var comparisonMessage = new List<string> {};

                if (comparisonAttributes.Contains("Price"))
                {
                    koiFishList = koiFishList.OrderBy(k => k.Price).ToList();
                    for (int i = 0; i < koiFishList.Count - 1; i++)
                    {
                        var currentKoi = koiFishList[i];
                        var nextKoi = koiFishList[i + 1];

                        if (currentKoi.Price < nextKoi.Price)
                        {
                            comparisonMessage.Add($"Cá koi {nextKoi.Name} đắt hơn cá koi {currentKoi.Name}.");
                        }
                        else if (currentKoi.Price == nextKoi.Price)
                        {
                            comparisonMessage.Add($"Cá koi {nextKoi.Name} có giá tương đương với cá koi {currentKoi.Name}.");
                        }
                        else 
                        {
                            comparisonMessage.Add($"Cá koi {nextKoi.Name} có giá tương đương với cá koi {currentKoi.Name}.");
                        }
                    }
                }

                if (comparisonAttributes.Contains("Size"))
                {
                    koiFishList = koiFishList.OrderBy(k => k.Size).ToList();
                    for (int i = 0; i < koiFishList.Count - 1; i++)
                    {
                        var currentKoi = koiFishList[i];
                        var nextKoi = koiFishList[i + 1];

                        if (!string.Equals(currentKoi.Size, nextKoi.Size, StringComparison.OrdinalIgnoreCase))
                        {
                            comparisonMessage.Add($"Cá koi {nextKoi.Name} lớn hơn cá koi {currentKoi.Name}.");
                        }
                        else
                        {
                            comparisonMessage.Add($"Cá koi {nextKoi.Name} có kích thước giống với cá koi {currentKoi.Name}.");
                        }
                    }
                }

                if (comparisonAttributes.Contains("HealthStatus"))
                {
                    for (int i = 0; i < koiFishList.Count - 1; i++)
                    {
                        var currentKoi = koiFishList[i];
                        var nextKoi = koiFishList[i + 1];

                        int currentKoiRank = GetHealthRank(currentKoi.HealthStatus);
                        int nextKoiRank = GetHealthRank(nextKoi.HealthStatus);

                        if (currentKoiRank > nextKoiRank)
                        {
                            comparisonMessage.Add($"Cá koi {currentKoi.Name} có tình trạng sức khỏe tốt hơn cá koi {nextKoi.Name}.");
                        }
                        else if (currentKoiRank < nextKoiRank)
                        {
                            comparisonMessage.Add($"Cá koi {nextKoi.Name} có tình trạng sức khỏe tốt hơn cá koi {currentKoi.Name}.");
                        }
                        else if (currentKoiRank == nextKoiRank)
                        {
                            comparisonMessage.Add($"Cá koi {currentKoi.Name} và cá koi {nextKoi.Name} có tình trạng sức khỏe tương đương.");
                        }
                    }
                }

                if (comparisonAttributes.Contains("Gender"))
                {
                    for (int i = 0; i < koiFishList.Count - 1; i++)
                    {
                        var currentKoi = koiFishList[i];
                        var nextKoi = koiFishList[i + 1];

                        if (!string.Equals(currentKoi.Gender, nextKoi.Gender, StringComparison.OrdinalIgnoreCase))
                        {
                            comparisonMessage.Add($"Cá koi {nextKoi.Name} có giới tính khác với cá koi  {currentKoi.Name}.");
                        }
                        else
                        {
                            comparisonMessage.Add($"Cá koi {nextKoi.Name} có nguồn gốc giống với cá koi {currentKoi.Name}.");
                        }
                    }
                }

                if (comparisonAttributes.Contains("FarmOrigin"))
                {
                    for (int i = 0; i < koiFishList.Count - 1; i++)
                    {
                        var currentKoi = koiFishList[i];
                        var nextKoi = koiFishList[i + 1];

                        if (!string.Equals(currentKoi.FarmOrigin, nextKoi.FarmOrigin, StringComparison.OrdinalIgnoreCase))
                        {
                            comparisonMessage.Add($"Cá koi {nextKoi.Name} có nguồn gốc khác với cá koi {currentKoi.Name}.");
                        }
                        else
                        {
                            comparisonMessage.Add($"Cá koi {nextKoi.Name} có nguồn gốc giống với cá koi {currentKoi.Name}.");
                        }
                    }
                }

                if (comparisonAttributes.Contains("Color"))
                {
                    for (int i = 0; i < koiFishList.Count - 1; i++)
                    {
                        var currentKoi = koiFishList[i];
                        var nextKoi = koiFishList[i + 1];

                        if (!string.Equals(currentKoi.Color, nextKoi.Color, StringComparison.OrdinalIgnoreCase))
                        {
                            comparisonMessage.Add($"Cá koi {nextKoi.Name} có màu sắc khác với cá koi {currentKoi.Name}.");
                        }
                        else
                        {
                            comparisonMessage.Add($"Cá koi {nextKoi.Name} có màu sắc giống với cá koi {currentKoi.Name}.");
                        }
                    }
                }

                if (comparisonAttributes.Contains("MaintenanceCost"))
                {
                    koiFishList = koiFishList.OrderBy(k => k.MaintenanceCost).ToList();
                    for (int i = 0; i < koiFishList.Count - 1; i++)
                    {
                        var currentKoi = koiFishList[i];
                        var nextKoi = koiFishList[i + 1];

                        if (currentKoi.MaintenanceCost < nextKoi.MaintenanceCost)
                        {
                            comparisonMessage.Add($"Cá koi {nextKoi.Name} có chi phí bảo trì cao hơn cá koi {currentKoi.Name}.");
                        }
                        else if (currentKoi.MaintenanceCost > nextKoi.MaintenanceCost)
                        {
                            comparisonMessage.Add($"Cá koi {currentKoi.Name} có chi phí bảo trì cao hơn cá koi {nextKoi.Name}.");
                        }
                        else
                        {
                            comparisonMessage.Add($"Cá koi {currentKoi.Name} và cá koi {nextKoi.Name} có chi phí bảo trì giống nhau.");
                        }
                    }
                }

                if (comparisonAttributes.Contains("BirthYear"))
                {
                    koiFishList = koiFishList.OrderBy(k => k.BirthYear).ToList();
                    for (int i = 0; i < koiFishList.Count - 1; i++)
                    {
                        var currentKoi = koiFishList[i];
                        var nextKoi = koiFishList[i + 1];

                        if (currentKoi.BirthYear < nextKoi.BirthYear)
                        {
                            comparisonMessage.Add($"Cá koi {nextKoi.Name} trẻ hơn cá koi {currentKoi.Name}.");
                        }
                        else if (currentKoi.BirthYear > nextKoi.BirthYear)
                        {
                            comparisonMessage.Add($"Cá koi {currentKoi.Name} trẻ hơn cá koi {nextKoi.Name}.");
                        }
                        else
                        {
                            comparisonMessage.Add($"Cá koi {currentKoi.Name} và cá koi {nextKoi.Name} có năm sinh giống nhau.");
                        }
                    }
                }

                var result = new
                {
                    KoiFishList = koiFishList,
                    ComparisonMessage = comparisonMessage
                };

                return new BusinessResult(Const.SUCCESS_CREATE_CODE, "So sánh thành công", result);
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }

        private int GetHealthRank(string healthStatus)
        {
            switch (healthStatus?.ToLower())
            {
                case "excellent":
                    return 3; 
                case "good":
                    return 2; 
                case "fair":
                    return 1; 
                case "poor":
                    return 0; 
                default:
                    return -1; 
            }
        }

    }
}

