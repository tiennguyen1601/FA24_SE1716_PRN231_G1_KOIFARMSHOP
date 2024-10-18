using KOIFARMSHOP.Common;
using KOIFARMSHOP.Data;
using KOIFARMSHOP.Data.Models;
using KOIFARMSHOP.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOIFARMSHOP.Service.Services
{
    public interface ICategoryService
    {
        Task<IBusinessResult> GetAll();
        Task<IBusinessResult> GetById(int categoryId);
        Task<IBusinessResult> Save(Category category);
        Task<IBusinessResult> DeleteById(int categoryId);
        Task<Category> GetCategoryById(int categoryId);
    }

    public class CategoryService : ICategoryService
    {
        private readonly UnitOfWork _unitOfWork;

        public CategoryService()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<IBusinessResult> DeleteById(int categoryId)
        {
            try
            {
                var currCategory = await _unitOfWork.CategoryRepository.GetByIdAsync(categoryId);

                if (currCategory == null)
                {
                    return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new Product());
                }
                else
                {
                    var result = await _unitOfWork.CategoryRepository.RemoveCategory(currCategory);

                    if (result)
                    {
                        return new BusinessResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG, currCategory);
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG, currCategory);
                    }
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IBusinessResult> GetAll()
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync();

            if (categories == null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<Category>());
            }
            else
            {
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, categories);
            }
        }

        public async Task<IBusinessResult> GetById(int categoryId)
        {
            var currCategory = await _unitOfWork.CategoryRepository.GetByIdAsync(categoryId);

            if (currCategory == null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<Category>());
            }
            else
            {
                return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, currCategory);
            }
        }

        public async Task<Category> GetCategoryById(int categoryId)
        {
            var currCategory = await _unitOfWork.CategoryRepository.GetByIdAsync(categoryId);

            return currCategory;
        }

        public async Task<IBusinessResult> Save(Category category)
        {
            try
            {
                int result = -1;
                var currCategory = _unitOfWork.ProductRepository.GetById(category.CategoryId);

                if (currCategory != null)
                {
                    #region businessR
                    #endregion businessR

                    result = await _unitOfWork.CategoryRepository.UpdateAsync(category);

                    if (result > 0)
                    {
                        return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, currCategory);
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG, currCategory);
                    }

                }
                else
                {
                    result = await _unitOfWork.CategoryRepository.CreateAsync(category);
                    if (result > 0)
                    {
                        return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, category);
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, category);
                    }
                }

            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }
    }
}
