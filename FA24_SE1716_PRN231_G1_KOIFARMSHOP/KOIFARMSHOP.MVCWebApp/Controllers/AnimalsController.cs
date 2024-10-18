using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using KOIFARMSHOP.Data.Models;
using KOIFARMSHOP.Common;
using Newtonsoft.Json;
using KOIFARMSHOP.Service.Base;

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
            var animals = await GetAnimals();
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
            if (!ModelState.IsValid)
            {
                await LoadStaffData();
                return View(animal);
            }

            var saveStatus = await CreateAnimal(animal);
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
            if (id != animal.AnimalId) return NotFound();

            if (!ModelState.IsValid)
            {
                await LoadStaffData();
                return View(animal);
            }

            var saveStatus = await UpdateAnimal(id, animal);
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
            return saveStatus ? RedirectToAction(nameof(Index)) : RedirectToAction(nameof(Index)); // Adjust handling as needed
        }

        private async Task<List<Animal>> GetAnimals()
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"{Const.APIEndPoint}Animals");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<BusinessResult>(content);
                if (result?.Data != null)
                {
                    return JsonConvert.DeserializeObject<List<Animal>>(result.Data.ToString());
                }
            }
            return new List<Animal>();
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

        private async Task<bool> CreateAnimal(Animal animal)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.PostAsJsonAsync($"{Const.APIEndPoint}Animals", animal);
            return response.IsSuccessStatusCode;
        }

        private async Task<bool> UpdateAnimal(int id, Animal animal)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.PutAsJsonAsync($"{Const.APIEndPoint}Animals/{id}", animal);
            return response.IsSuccessStatusCode;
        }

        private async Task<bool> DeleteAnimal(int id)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.DeleteAsync($"{Const.APIEndPoint}Animals/{id}");
            return response.IsSuccessStatusCode;
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
