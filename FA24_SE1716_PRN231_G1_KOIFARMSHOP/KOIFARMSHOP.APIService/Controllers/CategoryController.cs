using KOIFARMSHOP.Common;
using KOIFARMSHOP.Data.DTO.CategoryDTO;
using KOIFARMSHOP.Data.Models;
using KOIFARMSHOP.Service.Base;
using KOIFARMSHOP.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;

namespace KOIFARMSHOP.APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IBusinessResult> GetCategories()
        {
            return await _categoryService.GetAll();
        }

        [HttpGet("{categoryId}")]
        public async Task<IBusinessResult> ViewDetails(int categoryId)
        {
            return await _categoryService.GetById(categoryId);
        }

        [HttpPost]
        public async Task<IBusinessResult> CreateCategory(CategoryCreateReqModel categoryCreateReqModel)
        {
            Category newCategory = new Category
            {
                Name = categoryCreateReqModel.Name,
                Description = categoryCreateReqModel.Description,
                Status = "Active"
            };

            var result = await _categoryService.Save(newCategory);

            return result;
        }

        [HttpPut]
        public async Task<IBusinessResult> UpdateCateory(CategoryUpdateReqModel categoryUpdateReqModel)
        {
            var currCategory = await _categoryService.GetCategoryById(categoryUpdateReqModel.CategoryId);

            if (currCategory == null) return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<Category>());

            currCategory.Name = !string.IsNullOrEmpty(categoryUpdateReqModel.Name) ? categoryUpdateReqModel.Name : currCategory.Name;
            currCategory.Description = !string.IsNullOrEmpty(categoryUpdateReqModel.Description) ? categoryUpdateReqModel.Description : currCategory.Description;

            var result = await _categoryService.Save(currCategory);

            return result;
        }

        [HttpDelete("{categoryId}")]
        public async Task<IBusinessResult> DeleteCategory(int categoryId)
        {
            return await _categoryService.DeleteById(categoryId);
        }

        [HttpPost]
        [Route("ActivateDeactivate/{categoryId}")]
        public async Task<IBusinessResult> ActivateDeactivate(int categoryId)
        {
            return await _categoryService.ActicvateDeactivate(categoryId);
        }
    }
}
