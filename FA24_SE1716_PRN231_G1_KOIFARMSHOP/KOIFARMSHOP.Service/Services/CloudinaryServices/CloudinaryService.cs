using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using KOIFARMSHOP.Common;
using KOIFARMSHOP.Data.DTO.LoginDTO;
using KOIFARMSHOP.Service.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOIFARMSHOP.Service.Services.CloudinaryServices
{
    public interface ICloudinaryService
    {
        Task<ImageUploadResult> AddPhoto(IFormFile file);
        Task<DeletionResult> DeleteFile(string publicId);
        Task<IBusinessResult> AddPhotos(List<IFormFile> file);
    }
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(
               config.Value.CloudName,
               config.Value.ApiKey,
               config.Value.ApiSecret
               );
            _cloudinary = new Cloudinary(acc);
        }
        public async Task<ImageUploadResult> AddPhoto(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }
            return uploadResult;
        }

        public async Task<IBusinessResult> AddPhotos(List<IFormFile> file)
        {
            List<string> imageURLs = [];

            foreach (var item in file)
            {
                var uploadResult = new ImageUploadResult();
                if (item.Length > 0)
                {
                    using var stream = item.OpenReadStream();
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(item.FileName, stream),
                        Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                    };
                    uploadResult = await _cloudinary.UploadAsync(uploadParams);
                }

                imageURLs.Add(uploadResult.Url.ToString());
            }

            return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, imageURLs); ;
        }

        public async Task<DeletionResult> DeleteFile(string publicId)
        {
            var deleteParams = new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Video  // Specify that the resource type is video
            };
            var result = await _cloudinary.DestroyAsync(deleteParams);

            return result;
        }
    }
}
