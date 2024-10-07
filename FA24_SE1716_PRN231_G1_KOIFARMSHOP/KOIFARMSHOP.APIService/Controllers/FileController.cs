using KOIFARMSHOP.Service.Base;
using KOIFARMSHOP.Service.Services.CloudinaryServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace KOIFARMSHOP.APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly ICloudinaryService _cloudinaryService;

        public FileController(ICloudinaryService cloudinaryService)
        {
            _cloudinaryService = cloudinaryService;
        }

        [HttpPost]
        [Route("cloudinary/image")]
        public async Task<IBusinessResult> UploadFileImagesCloudinary(List<IFormFile> images)
        {
            var result = await _cloudinaryService.AddPhotos(images);

            return result;
        }
    }
}
