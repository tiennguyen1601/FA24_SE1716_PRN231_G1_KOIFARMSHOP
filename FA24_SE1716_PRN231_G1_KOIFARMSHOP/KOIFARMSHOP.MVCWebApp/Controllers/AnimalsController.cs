using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using KOIFARMSHOP.Data.Models;
using KOIFARMSHOP.Common;
using Newtonsoft.Json;
using KOIFARMSHOP.Service.Base;
using NuGet.Common;

namespace KOIFARMSHOP.MVCWebApp.Controllers
{
    public class AnimalsController : Controller
    {
        private readonly FA24_SE1716_PRN231_G1_KOIFARMSHOPContext _context;

        public AnimalsController(FA24_SE1716_PRN231_G1_KOIFARMSHOPContext context)
        {
            _context = context;
        }

        // GET: Animals
        public async Task<IActionResult> Index()
        {
            //var animals = await GetAnimals();
            //return View(animals);

            return View();
        }

        // GET: Animals/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0) return NotFound();

            var animal = await GetAnimal(id);
            if (animal == null) return NotFound();

            return View(animal);
        }

        // GET: Animals/Create
        public async Task<IActionResult> Create()
        {
            await LoadStaffData();
            return View();
        }

        // POST: Animals/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Animal animal)
        {
            var token = HttpContext.Session.GetString("Token");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Customers");
            }
            if (!ModelState.IsValid)
            {
                await LoadStaffData();
                return View(animal);
            }

            var saveStatus = await CreateAnimal(animal, token);
            if (saveStatus)
            {
                return RedirectToAction(nameof(Index));
            }

            await LoadStaffData();
            return View(animal);
        }

        // GET: Animals/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0) return NotFound();

            var animal = await GetAnimal(id);
            if (animal == null) return NotFound();

            await LoadStaffData();
            return View(animal);
        }

        // POST: Animals/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Animal animal)
        {
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Customers");
            }
            if (id != animal.AnimalId) return NotFound();

            if (!ModelState.IsValid)
            {
                await LoadStaffData();
                return View(animal);
            }

            var saveStatus = await UpdateAnimal(id, animal,token);
            if (saveStatus)
            {
                return RedirectToAction(nameof(Index));
            }

            await LoadStaffData();
            return View(animal);
        }

        // GET: Animals/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return NotFound();

            var animal = await GetAnimal(id);
            if (animal == null) return NotFound();

            return View(animal);
        }

        // POST: Animals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var saveStatus = await DeleteAnimal(id);
            return saveStatus ? RedirectToAction(nameof(Index)) : RedirectToAction(nameof(Index)); 
        }

        // GET: Animals/Compare
        public async Task<IActionResult> Compare()
        {
            var model = new CompareAnimalsViewModel
            {
                Animals = await GetAnimals(),
                SelectedAnimalIds = new List<int>(),
                ComparisonResults = new List<ComparisonResult>()
            };

            if (!model.Animals.Any())
            {
                ModelState.AddModelError("", "No animals available for comparison.");
                return View(model);
            }

            return View(model);
        }

        // POST: Animals/Compare
        [HttpPost]
        public async Task<IActionResult> Compare(CompareAnimalsViewModel model)
        {
            model.Animals = await GetAnimals();

            if (model.SelectedAnimalIds == null || !model.SelectedAnimalIds.Any())
            {
                ModelState.AddModelError("", "No animal IDs provided.");
                return View(model);
            }

            if (model.SelectedAttributes == null || !model.SelectedAttributes.Any())
            {
                ModelState.AddModelError("", "No attributes selected for comparison.");
                return View(model);
            }

            model.ComparisonResults = await CompareMultipleFishAttributes(model.SelectedAnimalIds, model.SelectedAttributes);


            if (model.ComparisonResults == null || !model.ComparisonResults.Any())
            {
                ModelState.AddModelError("", "No results to display for the selected attributes.");
                return View(model);
            }

            return View("ComparisonResults", model);
        }



        private async Task<List<Animal>> GetAnimals()
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"{Const.APIEndPoint}Animals");
            var animals = new List<Animal>();

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                if (result != null && result.Data != null)
                {
                    animals = JsonConvert.DeserializeObject<List<Animal>>(result.Data.ToString()) ?? new List<Animal>();
                }
            }
            return animals;
        }


        private async Task<Animal> GetAnimal(int id)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"{Const.APIEndPoint}Animals/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<BusinessResult>(content);
                if (result?.Data != null)
                {
                    return JsonConvert.DeserializeObject<Animal>(result.Data.ToString());
                }
            }
            return null;
        }

        private async Task<bool> CreateAnimal(Animal animal, string token)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await httpClient.PostAsJsonAsync($"{Const.APIEndPoint}Animals", animal);
            return response.IsSuccessStatusCode;
        }

        private async Task<bool> UpdateAnimal(int id, Animal animal, string token)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await httpClient.PutAsJsonAsync($"{Const.APIEndPoint}Animals/{id}", animal);
            return response.IsSuccessStatusCode;
        }

        private async Task<bool> DeleteAnimal(int id)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.DeleteAsync($"{Const.APIEndPoint}Animals/{id}");
            return response.IsSuccessStatusCode;
        }


        private static async Task<List<CompareAnimalsViewModel>> CompareMultipleFishAttributes(List<int> animalIds, List<string> selectedAttributes)
        {
            using var httpClient = new HttpClient();

            var response = await httpClient.PostAsJsonAsync($"{Const.APIEndPoint}Animals/CompareMultipleFish", animalIds, selectedAttributes);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                if (result?.Data != null)
                {
                    return JsonConvert.DeserializeObject<List<CompareAnimalsViewModel>>(result.Data.ToString());
                }
            }

            return new List<CompareAnimalsViewModel>();
        }


        private async Task LoadStaffData()
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"{Const.APIEndPoint}Staffs");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<BusinessResult>(content);
                if (result?.Data != null)
                {
                    var staff = JsonConvert.DeserializeObject<List<Staff>>(result.Data.ToString());
                    ViewData["CreatedBy"] = new SelectList(staff, "StaffId", "FullName");
                    ViewData["ModifiedBy"] = new SelectList(staff, "StaffId", "FullName");
                }
            }
        }
    }
}
