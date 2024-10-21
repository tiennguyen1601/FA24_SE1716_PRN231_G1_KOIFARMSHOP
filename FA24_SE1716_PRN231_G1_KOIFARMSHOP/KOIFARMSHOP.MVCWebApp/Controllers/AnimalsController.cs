using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using KOIFARMSHOP.Data.Models;
using KOIFARMSHOP.Common;
using Newtonsoft.Json;
using KOIFARMSHOP.Service.Base;
using NuGet.Common;
using KOIFARMSHOP.MVCWebApp.Models;
using Microsoft.EntityFrameworkCore;

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

        // GET: Animals/CustomerView
        public async Task<IActionResult> CustomerView()
        {
            return View(); 
        }

        public async Task<IActionResult> Compare()
        {
            // Lấy danh sách các cá Koi từ API
            var animals = await GetAnimals();

            // Trả về View với ViewModel chứa danh sách cá
            return View(animals);
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

        public async Task<IActionResult> PerformComparison(int[] animalIds, string[] selectedAttributes)
        {
            var requestBody = new
            {
                ids = animalIds.ToList(),
                comparisonAttributes = selectedAttributes.ToList()
            };

            using var httpClient = new HttpClient();
            var response = await httpClient.PostAsJsonAsync($"{Const.APIEndPoint}Animals/CompareMultipleFish", requestBody);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<BusinessResult>(content);

                if (apiResponse != null && apiResponse.Data != null)
                {
                    var comparisonData = JsonConvert.DeserializeObject<ComparisonData>(apiResponse.Data.ToString());

                    // Tạo ComparisonResults từ koiFishList và selectedAttributes
                    var comparisonResults = new List<ComparisonResult>();
                    foreach (var attribute in selectedAttributes)
                    {
                        var result = new ComparisonResult { AttributeName = attribute };

                        foreach (var koiFish in comparisonData.KoiFishList)
                        {
                            var attributeValue = GetAnimalAttribute(koiFish, attribute);
                            result.Values[koiFish.AnimalId] = attributeValue;
                        }

                        comparisonResults.Add(result);
                    }

                    var viewModel = new CompareAnimalsViewModel
                    {
                        Animals = comparisonData.KoiFishList,
                        ComparisonMessages = comparisonData.ComparisonMessage,
                        ComparisonResults = comparisonResults // Đảm bảo ComparisonResults đã được tạo
                    };

                    return View("ComparisonResults", viewModel);
                }
            }

            ModelState.AddModelError("", "Có lỗi xảy ra khi so sánh các cá koi.");
            return RedirectToAction("Compare");
        }


        private async Task<List<Animal>> GetAnimals()
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"{Const.APIEndPoint}Animals/get-Animal-by-user");
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


        private string GetAnimalAttribute(Animal animal, string attribute)
        {
            return attribute switch
            {
                "Size" => animal.Size ?? "Unknown", 
                "Color" => animal.Color ?? "Unknown",
                "Price" => animal.Price?.ToString("C") ?? "N/A", 
                "HealthStatus" => animal.HealthStatus ?? "Unknown",
                "Gender" => animal.Gender ?? "Unknown",
                "FarmOrigin" => animal.FarmOrigin ?? "Unknown",
                "MaintenanceCost" => animal.MaintenanceCost?.ToString("C") ?? "N/A", 
                "BirthYear" => animal.BirthYear?.ToString() ?? "N/A",
                _ => "N/A" 
            };
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
