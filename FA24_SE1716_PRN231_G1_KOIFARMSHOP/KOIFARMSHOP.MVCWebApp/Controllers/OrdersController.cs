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
        // GET: Orders/Create
        public async Task<IActionResult> Create(int? animalId, string animalName, int? productId, string productName)
        {
            var animal = await GetAnimalById(animalId);
            var promotions = await GetPromotion();

            ViewData["PromotionId"] = new SelectList(promotions, "PromotionId", "Title");
            ViewData["AnimalName"] = animalName ?? "Unknown";  
            ViewData["AnimalId"] = animalId ?? 0;
            ViewData["ProductName"] = productName ?? "Unknown";
            ViewData["ProductId"] = productId ?? 0;

            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderCompleteRequest order, int? animalId, int? productId)
        {
            var promotions = await GetPromotion(); 
            
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

                var response = await httpClient.PostAsJsonAsync($"{Const.APIEndPoint}Orders", order);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                    if (result != null && result.Status == Const.SUCCESS_CREATE_CODE)
                    {
                        saveStatus = true;
                    }
                    else if (result != null && result.Message.Contains("Out of quantity"))
                    {
                        ModelState.AddModelError("", result.Message);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "An error occurred while creating the order.");
                }
            }

            if (!saveStatus)
            {
                var animal = await GetAnimalById(animalId ?? 0);
                var product = await GetProductById(productId ?? 0);

                ViewData["PromotionId"] = new SelectList(promotions, "PromotionId", "Title");
                ViewData["AnimalId"] = animalId ?? 0;
                ViewData["ProductId"] = productId ?? 0; 

                ViewData["AnimalName"] = animal?.Name ?? "Unknown";
                ViewData["ProductName"] = product?.Name ?? "Unknown";

                order.AnimalID = animalId;
                order.ProductID = productId;
                order.Quantity = order.Quantity;

                return View(order);
            }
            return RedirectToAction(nameof(Index));
        }




        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var promotions = await GetPromotion();

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
                                PromotionId = data.PromotionId,
                                ShippingAddress = data.ShippingAddress,
                                DeliveryMethod = data.DeliveryMethod,
                            };
                            ViewData["PromotionId"] = new SelectList(promotions, "PromotionId", "Title");
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
            var promotions = await GetPromotion();
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
                ViewData["PromotionId"] = new SelectList(promotions, "PromotionId", "Title");
                return RedirectToAction(nameof(Index));
            }
            return View(order); 
        }


        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

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



        public async Task<Product> GetProductById(int? id)
        {
            var products = new Product();
            using (var httpClient = new HttpClient())
            {
                using (var res = await httpClient.GetAsync($"{Const.APIEndPoint}Products/{id}"))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        var content = await res.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                        if (result != null && result.Data != null)
                        {
                            products = JsonConvert.DeserializeObject<Product>(result.Data.ToString());

                        }
                    }
                }
            }
            return products;
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
        public async Task<Animal> GetAnimalById(int? id)
        {
            var animals = new Animal();
            using (var httpClient = new HttpClient())
            {
                using (var res = await httpClient.GetAsync($"{Const.APIEndPoint}Animals/{id}"))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        var content = await res.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                        if (result != null && result.Data != null)
                        {
                            animals = JsonConvert.DeserializeObject<Animal>(result.Data.ToString());

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
