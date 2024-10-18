using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KOIFARMSHOP.Data.Models;
using KOIFARMSHOP.Common;
using Newtonsoft.Json;
using KOIFARMSHOP.Service.Base;

namespace KOIFARMSHOP.MVCWebApp.Controllers
{
    public class AnimalsController : Controller
    {
        private readonly FA24_SE1716_PRN231_G1_KOIFARMSHOPContext _context;
        private readonly HttpClient _httpClient;

        public AnimalsController(FA24_SE1716_PRN231_G1_KOIFARMSHOPContext context)
        {
            _context = context;
            _httpClient = new HttpClient();
        }

        // GET: Animals
        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{Const.APIEndPoint}Animals");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                    if (result?.Data != null)
                    {
                        var data = JsonConvert.DeserializeObject<List<Animal>>(result.Data.ToString());
                        return View(data);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error fetching animals: {ex.Message}";
            }
            return View(new List<Animal>());
        }

        // GET: Animals/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0) return NotFound();

            try
            {
                var response = await _httpClient.GetAsync($"{Const.APIEndPoint}Animals/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                    if (result?.Data != null)
                    {
                        var animal = JsonConvert.DeserializeObject<Animal>(result.Data.ToString());
                        return View(animal);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error fetching animal details: {ex.Message}";
            }
            return View(new Animal());
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

            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{Const.APIEndPoint}Animals", animal);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error creating animal: {ex.Message}";
            }

            await LoadStaffData();
            return View(animal);
        }

        // GET: Animals/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0) return NotFound();

            Animal animal = null;
            try
            {
                var response = await _httpClient.GetAsync($"{Const.APIEndPoint}Animals/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                    if (result?.Data != null)
                    {
                        animal = JsonConvert.DeserializeObject<Animal>(result.Data.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error fetching animal data: {ex.Message}";
            }

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

            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{Const.APIEndPoint}Animals/{id}", animal);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error updating animal: {ex.Message}";
            }

            await LoadStaffData();
            return View(animal);
        }

        // GET: Animals/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return NotFound();

            try
            {
                var response = await _httpClient.DeleteAsync($"{Const.APIEndPoint}Animals/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error deleting animal: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadStaffData()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{Const.APIEndPoint}Staffs");
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
            catch (Exception ex)
            {
                ViewBag.Error = $"Error loading staff data: {ex.Message}";
            }
        }
    }
}
