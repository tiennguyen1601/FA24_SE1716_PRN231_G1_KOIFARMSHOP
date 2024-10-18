using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KOIFARMSHOP.Data.Models;
using KOIFARMSHOP.Common;
using KOIFARMSHOP.Data.DTO.OrderDTO;
using KOIFARMSHOP.Service.Base;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace KOIFARMSHOP.MVCWebApp.Controllers
{
    public class OrdersController : Controller
    {
        private readonly FA24_SE1716_PRN231_G1_KOIFARMSHOPContext _context;

        public OrdersController(FA24_SE1716_PRN231_G1_KOIFARMSHOPContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            using (var httpClient = new HttpClient())
            {
                using (var respone = await httpClient.GetAsync(Const.APIEndPoint + "Orders"))
                {
                    if (respone.IsSuccessStatusCode)
                    {
                        var content = await respone.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                        if (result != null && result.Data != null)
                        {
                            var data = JsonConvert.DeserializeObject<List<OrderResponseModel>>(result.Data.ToString());
                            return View(data);
                        }
                    }

                }
            }
            return View(new List<OrderResponseModel>());
        }


        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var respone = await httpClient.GetAsync(Const.APIEndPoint + "Orders/" + id))
                {
                    if (respone.IsSuccessStatusCode)
                    {
                        var content = await respone.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                        if (result != null && result.Data != null)
                        {
                            var data = JsonConvert.DeserializeObject<OrderResponseModel>(result.Data.ToString());
                            return View(data);
                        }
                    }

                }
            }
            return View(new OrderResponseModel());
        }


        // GET: Orders/Create
        public async Task<IActionResult> Create()
        {
            var customers = await GetCustomer();
            var promotions = await GetPromotion();
            var animals = await GetAnimal();
            //var products = await GetProduct();

            ViewData["CustomerId"] = new SelectList(customers, "CustomerId", "Name");
            ViewData["PromotionId"] = new SelectList(promotions, "PromotionId", "Title");
            //ViewData["ProductId"] = new SelectList(products, "ProductId", "ProductName");
            ViewData["AnimalId"] = new SelectList(animals, "AnimalId", "Species");

            return View();

        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( OrderCompleteRequest order)
        {

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(order);
            }

            var customers = await GetCustomer();
            var promotions = await GetPromotion();
            var animals = await GetAnimal();

            bool saveStatus = false;
            var token = HttpContext.Session.GetString("Token");

            if (string.IsNullOrEmpty(token))
            {
                ModelState.AddModelError("", "User is not authenticated.");
                return View(order);
            }




            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                Console.WriteLine(JsonConvert.SerializeObject(order));

                var response = await httpClient.PostAsJsonAsync(Const.APIEndPoint + "Orders", order);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                    if (result != null && result.Status == Const.SUCCESS_CREATE_CODE)
                    {
                        saveStatus = true;
                    }
                }
            }

            if (saveStatus)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewData["CustomerId"] = new SelectList(customers, "CustomerId", "Name");
            ViewData["PromotionId"] = new SelectList(promotions, "PromotionId", "Title");
            ViewData["AnimalId"] = new SelectList(animals, "AnimalId", "Species");

            return View(order);
        }



        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Name", order.CustomerId);
            ViewData["PromotionId"] = new SelectList(_context.Promotions, "PromotionId", "Title", order.PromotionId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,CustomerId,OrderDate,TotalAmount,PromotionId,ShippingAddress,DeliveryMethod,PaymentStatus,Vat,TotalAmountVat,Status")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Name", order.CustomerId);
            ViewData["PromotionId"] = new SelectList(_context.Promotions, "PromotionId", "Title", order.PromotionId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Promotion)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }



        public async Task<List<Customer>> GetCustomer()
        {
            var customers = new List<Customer>();
            using (var httpClient = new HttpClient())
            {
                using (var res = await httpClient.GetAsync(Const.APIEndPoint + "Customers"))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        var content = await res.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                        if (result != null && result.Data != null)
                        {
                            customers = JsonConvert.DeserializeObject<List<Customer>>(result.Data.ToString());

                        }
                    }
                }
            }
            return customers;
        }


        public async Task<List<Promotion>> GetPromotion()
        {
            var promotions = new List<Promotion>();
            using (var httpClient = new HttpClient())
            {
                using (var res = await httpClient.GetAsync(Const.APIEndPoint + "Promotions"))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        var content = await res.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                        if (result != null && result.Data != null)
                        {
                            promotions = JsonConvert.DeserializeObject<List<Promotion>>(result.Data.ToString());

                        }
                    }
                }
            }
            return promotions;
        }
        public async Task<List<Animal>> GetAnimal()
        {
            var animals = new List<Animal>();
            using (var httpClient = new HttpClient())
            {
                using (var res = await httpClient.GetAsync(Const.APIEndPoint + "Animals"))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        var content = await res.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                        if (result != null && result.Data != null)
                        {
                            animals = JsonConvert.DeserializeObject<List<Animal>>(result.Data.ToString());

                        }
                    }
                }
            }
            return animals;
        }
        public async Task<List<Product>> GetProduct()
        {
            var products = new List<Product>();
            using (var httpClient = new HttpClient())
            {
                using (var res = await httpClient.GetAsync(Const.APIEndPoint + "Products"))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        var content = await res.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                        if (result != null && result.Data != null)
                        {
                            products = JsonConvert.DeserializeObject<List<Product>>(result.Data.ToString());

                        }
                    }
                }
            }
            return products;
        }

    }

}
