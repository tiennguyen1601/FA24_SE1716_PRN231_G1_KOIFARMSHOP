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
            return View();
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

            var customers = await GetCustomer();
            var promotions = await GetPromotion();
            var animals = await GetAnimal();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.APIEndPoint + "Orders/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                        if (result != null && result.Data != null)
                        {
                            var data = JsonConvert.DeserializeObject<OrderResponseModel>(result.Data.ToString());

                            var totalAmount = data.OrderDetails.Sum(od => od.Amount ?? 0);
                            var totalSubtotal = data.OrderDetails.Sum(od => od.Subtotal ?? 0);
                            var totalDiscount = data.OrderDetails.Sum(od => od.Discount ?? 0);


                            var orderCompleteRequest = new OrderCompleteRequest
                            {
                                OrderId = data.OrderId,
                                TotalAmount = data.TotalAmount,
                                PromotionId = data.PromotionId,
                                ShippingAddress = data.ShippingAddress,
                                DeliveryMethod = data.DeliveryMethod,
                                PaymentStatus = data.PaymentStatus,
                                Vat = data.Vat,
                                Amount = totalAmount, 
                                Subtotal = totalSubtotal,
                                Discount = totalDiscount
                            };

                            ViewData["CustomerId"] = new SelectList(customers, "CustomerId", "Name");
                            ViewData["PromotionId"] = new SelectList(promotions, "PromotionId", "Title");
                            ViewData["AnimalId"] = new SelectList(animals, "AnimalId", "Species");

                            return View(orderCompleteRequest); 
                        }
                    }
                }
            }
            return View(new OrderCompleteRequest());
        }


        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrderCompleteRequest order) 
        {
            if (id != order.OrderId)
            {
                return NotFound();
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

                var response = await httpClient.PutAsJsonAsync(Const.APIEndPoint + "Orders/{id}", order);

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


        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customers = await GetCustomer();
            var promotions = await GetPromotion();
            var animals = await GetAnimal();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.APIEndPoint + "Orders/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                        if (result != null && result.Data != null)
                        {
                            var data = JsonConvert.DeserializeObject<OrderResponseModel>(result.Data.ToString());
                            ViewData["CustomerId"] = new SelectList(customers, "CustomerId", "Name");
                            ViewData["PromotionId"] = new SelectList(promotions, "PromotionId", "Title");
                            ViewData["AnimalId"] = new SelectList(animals, "AnimalId", "Species");
                            return View(data); 
                        }
                    }
                }
            }
            return View(new OrderResponseModel());
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool saveStatus = false;
            var product = new Product();
            using (var httpClient = new HttpClient())
            {
                using (var res = await httpClient.DeleteAsync(Const.APIEndPoint + $"Orders/{id}"))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        var content = await res.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                        if (result != null && result.Status == Const.SUCCESS_DELETE_CODE)
                        {
                            saveStatus = true;
                        }
                        else
                        {
                            saveStatus = false;
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
                return RedirectToAction();
            }
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
