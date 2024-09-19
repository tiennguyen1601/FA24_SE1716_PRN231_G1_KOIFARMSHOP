using KOIFARMSHOP.Service.Base;
using KOIFARMSHOP.Data;
using KOIFARMSHOP.Common;
using KOIFARMSHOP.Data.Models;
using System.Collections.Generic;
namespace KOIFARMSHOP.Service.Services
{
    public interface IAnimalService
    {
        Task<IBusinessResult> GetAll();
        Task<IBusinessResult> GetByID(string id);
        Task<IBusinessResult> Save(Animal animal);
        Task<IBusinessResult> DeleteByID(string id);
    }
    public class AnimalService : IAnimalService
    {
        private readonly UnitOfWork _unitOfWork;
        public AnimalService()
        {
            _unitOfWork ??= new UnitOfWork();
        }
        public async Task<IBusinessResult> GetAll() {
            var list = _unitOfWork.AnimalImageRepository.GetAllAsync();
            if (list == null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG,new List<Animal>());
            }
            else
            {
                return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, list);
            }
        }
        public async Task<IBusinessResult> GetByID(string id)
        {
            #region Business rule
            #endregion
            var list = _unitOfWork.AnimalImageRepository.GetByIdAsync(id);
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

        public async Task<IBusinessResult> DeleteByID(string id)
        {
            try
            {
                var animalById = await _unitOfWork.AnimalRepository.GetByIdAsync(id);
                if (animalById == null) {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new Animal());
                }else
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
            }catch (Exception ex) { return new BusinessResult(Const.ERROR_EXCEPTION, ex.ToString()); }
        }

    }
}
