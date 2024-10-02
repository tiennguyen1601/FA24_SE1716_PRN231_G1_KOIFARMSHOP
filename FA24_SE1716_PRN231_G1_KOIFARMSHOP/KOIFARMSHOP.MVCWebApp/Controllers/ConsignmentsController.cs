using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KOIFARMSHOP.Data.Models;
using KOIFARMSHOP.Common;
using KOIFARMSHOP.Service.Base;
using Newtonsoft.Json;
using Azure;

namespace KOIFARMSHOP.MVCWebApp.Controllers
{
    public class ConsignmentsController : Controller
    {
        private readonly FA24_SE1716_PRN231_G1_KOIFARMSHOPContext _context;

        public ConsignmentsController(FA24_SE1716_PRN231_G1_KOIFARMSHOPContext context)
        {
            _context = context;
        }

        // GET: Consignments
        public async Task<IActionResult> Index()
        {
            using (var httpClient = new HttpClient())
            {
                using (var respone = await httpClient.GetAsync(Const.APIEndPoint + "Consignments"))
                {
                    if (respone.IsSuccessStatusCode)
                    {
                        var content = await respone.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                        if (result != null && result.Data != null)
                        {
                            var data = JsonConvert.DeserializeObject<List<Consignment>>(result.Data.ToString());
                            return View(data);
                        }
                    }

                }
            }
            return View(new List<Consignment>());
        }

        // GET: Consignments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var consignment = new Consignment();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.APIEndPoint + "Consignments/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                        if (result != null && result.Data != null)
                        {
                            consignment = JsonConvert.DeserializeObject<Consignment>(result.Data.ToString());
                        }
                    }
                }
            }

            ViewData["AnimalId"] = new SelectList(_context.Animals, "AnimalId", "AnimalId", consignment.AnimalId);
            ViewData["CustomerName"] = new SelectList(_context.Customers, "CustomerId", "Name", consignment.CustomerId);
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", consignment.OrderId);

            return View(consignment);
        }


        // GET: Consignments/Create
        public IActionResult Create()
        {
            ViewData["AnimalId"] = new SelectList(_context.Animals, "AnimalId", "AnimalId");
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Name");
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId");
            return View();
        }

        // POST: Consignments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("ConsignmentId,CustomerId,AnimalId,ConsignmentType,StartDate,EndDate,Price,Status,CreatedAt,UpdatedAt,Notes,CommissionRate,OrderId")] Consignment consignment)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(consignment);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["AnimalId"] = new SelectList(_context.Animals, "AnimalId", "AnimalId", consignment.AnimalId);
        //    ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Name", consignment.CustomerId);
        //    ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", consignment.OrderId);
        //    return View(consignment);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Consignment consignment)
        {
            bool saveStatus = false;
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var respone = await httpClient.PostAsJsonAsync(Const.APIEndPoint + "Consignments/", consignment))
                    {
                        if (respone.IsSuccessStatusCode)
                        {
                            var content = await respone.Content.ReadAsStringAsync();
                            var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                            if (result != null && result.Status == Const.SUCCESS_CREATE_CODE)
                            {
                                saveStatus = true;
                            }
                            else { saveStatus = false; }
                        }
                    }
                }
            }

            if (saveStatus)
            {
                return RedirectToAction(nameof(Index));
            }
            else 
            { 
               return View(consignment);   
            }
        }

        // GET: Consignments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var consignment = new Consignment();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.APIEndPoint + "Consignments/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                        if (result != null && result.Data != null)
                        {
                            consignment = JsonConvert.DeserializeObject<Consignment>(result.Data.ToString());
                        }
                    }
                }
            }

            ViewData["AnimalId"] = new SelectList(_context.Animals, "AnimalId", "AnimalId", consignment.AnimalId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Name", consignment.CustomerId);
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", consignment.OrderId);
            return View(consignment);
        }

        // POST: Consignments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ConsignmentId,CustomerId,AnimalId,ConsignmentType,StartDate,EndDate,Price,Status,CreatedAt,UpdatedAt,Notes,CommissionRate,OrderId")] Consignment consignment)
        {
            bool saveStatus = false;

            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var respone = await httpClient.PutAsJsonAsync(Const.APIEndPoint + "Consignments/", consignment))
                    {
                        if (respone.IsSuccessStatusCode)
                        {
                            var content = await respone.Content.ReadAsStringAsync();
                            var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                            if (result != null && result.Status == Const.SUCCESS_UPDATE_CODE)
                            {
                                saveStatus = true;
                            }
                            else { saveStatus = false; }
                        }
                    }
                }
            }

            if (saveStatus)
            {
                return RedirectToAction(nameof(Index));
            }
            else {
                ViewData["AnimalId"] = new SelectList(_context.Animals, "AnimalId", "AnimalId", consignment.AnimalId);
                ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Name", consignment.CustomerId);
                ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", consignment.OrderId);
                return View(consignment);
            }    
        }

        // GET: Consignments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consignment = new Consignment();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.APIEndPoint + "Consignments/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                        if (result != null && result.Data != null)
                        {
                            consignment = JsonConvert.DeserializeObject<Consignment>(result.Data.ToString());
                        }
                    }
                }
            }

            if (consignment == null)
            {
                return NotFound();
            }

            return View(consignment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool deleteStatus = false;

            var consignment = new Consignment();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.APIEndPoint + "Consignments/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                        if (result != null && result.Data != null)
                        {
                            consignment = JsonConvert.DeserializeObject<Consignment>(result.Data.ToString());

                            consignment.Status = "Deleted";

                            using (var updateResponse = await httpClient.PutAsJsonAsync(Const.APIEndPoint + "Consignments/", consignment))
                            {
                                if (updateResponse.IsSuccessStatusCode)
                                {
                                    var updateContent = await updateResponse.Content.ReadAsStringAsync();
                                    var updateResult = JsonConvert.DeserializeObject<BusinessResult>(updateContent);

                                    if (updateResult != null && updateResult.Status == Const.SUCCESS_UPDATE_CODE)
                                    {
                                        deleteStatus = true;
                                    }
                                }
                            }
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
                // Nếu xóa thất bại, quay lại trang hiện tại
                ViewData["AnimalId"] = new SelectList(_context.Animals, "AnimalId", "AnimalId", consignment.AnimalId);
                ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Name", consignment.CustomerId);
                ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", consignment.OrderId);
                return View(consignment);
            }
        }



        private bool ConsignmentExists(int id)
        {
            return _context.Consignments.Any(e => e.ConsignmentId == id);
        }
    }
}
