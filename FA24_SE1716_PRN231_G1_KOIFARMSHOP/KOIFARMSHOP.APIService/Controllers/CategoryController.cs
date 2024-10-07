using KOIFARMSHOP.Data.Models;
using KOIFARMSHOP.Service.Base;
using KOIFARMSHOP.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IBusinessResult> CreateCategory(Category category)
        {
            return await _categoryService.Save(category);
        }

        [HttpPut]
        public async Task<IBusinessResult> UpdateCateory(Category category)
        {
            return await _categoryService.Save(category);
        }

        [HttpDelete("{categoryId}")]
        public async Task<IBusinessResult> DeleteCategory(int categoryId)
        {
            return await _categoryService.DeleteById(categoryId);
        }
    }
}
