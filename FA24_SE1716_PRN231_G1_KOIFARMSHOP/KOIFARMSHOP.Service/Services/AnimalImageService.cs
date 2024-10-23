using KOIFARMSHOP.Data.Models;
using KOIFARMSHOP.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOIFARMSHOP.Service.Services
{
    public interface IAnimalImageService
    {
        Task SaveAnimalImage(int animalId, List<string>? images);
    }
    public class AnimalImageService : IAnimalImageService
    {
        private readonly UnitOfWork _unitOfWork;
        public AnimalImageService()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task SaveAnimalImage(int animalId, List<string>? images)
        {
            var currAnimal = await _unitOfWork.AnimalRepository.GetByIdAsync(animalId);
        if (currAnimal == null) return;

            var currAnimalImages = await _unitOfWork.AnimalImageRepository.GetAnimalImagesByAnimalId(currAnimal.AnimalId);

            foreach (var item in currAnimalImages)
            {
                await _unitOfWork.AnimalImageRepository.RemoveAsync(item);
            }

            foreach (var image in images)
            {
                AnimalImage newAnimalImage = new AnimalImage
                {
                    AnimalId = currAnimal.AnimalId,
                    ImageUrl = image
                };

                await _unitOfWork.AnimalImageRepository.CreateAsync(newAnimalImage);
            }
        }
    }
}
