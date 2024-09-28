using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KOIFARMSHOP.Data.Models;
using KOIFARMSHOP.Common;
using Newtonsoft.Json;
using KOIFARMSHOP.Service.Base;
using System.Net.Http;
using System.Data;

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
            using (var httpClient = new HttpClient())
            {
                using (var respone = await httpClient.GetAsync(Const.APIEndPoint + "Animals"))
                { 
                    if (respone.IsSuccessStatusCode)
                    {
                        var content = await respone.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                        if(result != null && result.Data != null)
                        {
                            var data = JsonConvert.DeserializeObject<List<Animal>>(result.Data.ToString());
                            return View(data);
                        }
                    }
            
                }
            }
            return View(new List<Animal>());
        }

        // GET: Animals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var animal = await _context.Animals
            //    .Include(a => a.CreatedByNavigation)
            //    .Include(a => a.ModifiedByNavigation)
            //    .FirstOrDefaultAsync(m => m.AnimalId == id);
            //if (animal == null)
            //{
            //    return NotFound();
            //}

            //return View(animal);
            using (var httpClient = new HttpClient())
            {
                using (var respone = await httpClient.GetAsync(Const.APIEndPoint + "Animals/" + id))
                {
                    if (respone.IsSuccessStatusCode)
                    {
                        var content = await respone.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                        if (result != null && result.Data != null)
                        {
                            var data = JsonConvert.DeserializeObject<Animal>(result.Data.ToString());
                            return View(data);
                        }
                    }

                }
            }
            return View(new Animal());


        }

        // GET: Animals/Create
        public async  Task<IActionResult> Create()
        {
            ViewData["CreatedBy"] = new SelectList(_context.Staff, "StaffId", "FullName");
                ViewData["ModifiedBy"] = new SelectList(_context.Staff, "StaffId", "FullName");
               return View();
        }


        // POST: Animals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Animal animal)
        {


            //    if (ModelState.IsValid)
            //    {
            //        _context.Add(animal);
            //        await _context.SaveChangesAsync();
            //        return RedirectToAction(nameof(Index));
            //    }
            //    ViewData["CreatedBy"] = new SelectList(_context.Staff, "StaffId", "FullName", animal.CreatedBy);
            //    ViewData["ModifiedBy"] = new SelectList(_context.Staff, "StaffId", "FullName", animal.ModifiedBy);
            //    return View(animal);
            bool savaStatus = false;

            using (var httpClient = new HttpClient())
            {
                using (var respone = await httpClient.PostAsJsonAsync(Const.APIEndPoint + "Animals", animal))
                {
                    if (respone.IsSuccessStatusCode)
                    {
                        var content = await respone.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                        if (result != null && result.Status == Const.SUCCESS_CREATE_CODE)
                        {
                            savaStatus = true;
                        }
                        else
                        {
                            savaStatus = false;
                        }
                    }


                }
            }

            if (savaStatus)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewData["CreatedBy"] = new SelectList(_context.Staff, "StaffId", "FullName");
                ViewData["ModifiedBy"] = new SelectList(_context.Staff, "StaffId", "FullName");
                return View(animal);
            }


        }

        // GET: Animals/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            Animal animal = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.APIEndPoint + $"Animals/{id}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                        if (result != null && result.Data != null)
                        {
                            animal = JsonConvert.DeserializeObject<Animal>(result.Data.ToString());
                        }
                    }
                }
            }

            if (animal == null)
            {
                return NotFound();
            }

            ViewData["CreatedBy"] = new SelectList(_context.Staff, "StaffId", "FullName", animal.CreatedBy);
            ViewData["ModifiedBy"] = new SelectList(_context.Staff, "StaffId", "FullName", animal.ModifiedBy);
            return View(animal);
        }


        // POST: Animals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AnimalId,Origin,Species,Type,Gender,Size,Certificate,Price,Status,CreatedAt,UpdatedAt,MaintenanceCost,Color,AmountFeed,HealthStatus,FarmOrigin,BirthYear,Description,CreatedBy,ModifiedBy")] Animal animal)
        {
            

            bool updateStatus = false;

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PutAsJsonAsync(Const.APIEndPoint + $"Animals/{id}", animal))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                        if (result != null && result.Status == Const.SUCCESS_UPDATE_CODE)
                        {
                            updateStatus = true;
                        }
                        else
                        {
                            updateStatus = false;
                        }
                    }
                }
            }

            if (updateStatus)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewData["CreatedBy"] = new SelectList(_context.Staff, "StaffId", "FullName", animal.CreatedBy);
                ViewData["ModifiedBy"] = new SelectList(_context.Staff, "StaffId", "FullName", animal.ModifiedBy);
                return View(animal);
            }
        }


        // GET: Animals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var animal = new Animal();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.APIEndPoint + $"Animals/{id}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                        if (result != null && result.Status == Const.SUCCESS_UPDATE_CODE)
                        {
                           animal = JsonConvert.DeserializeObject<Animal>(result.Data.ToString());
                        }
                        
                    }
                }
            }
            return View(animal);
        }

        // POST: Animals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool deleteStatus = false;

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync(Const.APIEndPoint + $"Animals/{id}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                        if (result != null && result.Status == Const.SUCCESS_DELETE_CODE)
                        {
                            deleteStatus = true;
                        }
                        else
                        {
                            deleteStatus = false;
                        }
                    }
                }
            }

            if (deleteStatus)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(Delete)) ; 
            }
        }

        private bool AnimalExists(int id)
        {
            return _context.Animals.Any(e => e.AnimalId == id);
        }

    }
}
