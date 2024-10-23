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
using KOIFARMSHOP.Data.DTO.ProductDTO;
using KOIFARMSHOP.Data.Enums;

namespace KOIFARMSHOP.MVCWebApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly FA24_SE1716_PRN231_G1_KOIFARMSHOPContext _context;

        public ProductsController(FA24_SE1716_PRN231_G1_KOIFARMSHOPContext context)
        {
            _context = context;
        }
        public async Task<List<Category>> GetCategories()
        {
            var categories = new List<Category>();
            using (var httpClient = new HttpClient())
            {
                using (var res = await httpClient.GetAsync(Const.APIEndPoint + "Category"))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        var content = await res.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                        if (result != null && result.Data != null)
                        {
                            categories = JsonConvert.DeserializeObject<List<Category>>(result.Data.ToString());

                        }
                    }
                }
            }
            return categories.Where(x => x.Status.Equals(StatusEnums.Active.ToString())).ToList();
        }

        public async Task<Product> GetProduct(int productId)
        {
            var product = new Product();
            using (var httpClient = new HttpClient())
            {
                using (var res = await httpClient.GetAsync(Const.APIEndPoint + $"Products/{productId}"))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        var content = await res.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                        if (result != null && result.Data != null)
                        {
                            product = JsonConvert.DeserializeObject<Product>(result.Data.ToString());
                        }
                    }
                }
            }
            return product;
        }

        public async Task<List<Staff>> GetStaffs()
        {
            var staffs = new List<Staff>();
            using (var httpClient = new HttpClient())
            {
                using (var res = await httpClient.GetAsync(Const.APIEndPoint + "Staffs"))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        var content = await res.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                        if (result != null && result.Data != null)
                        {
                            staffs = JsonConvert.DeserializeObject<List<Staff>>(result.Data.ToString());

                        }
                    }
                }
            }
            return staffs;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            //using (var httpClient = new HttpClient())
            //{
            //    using (var res = await httpClient.GetAsync(Const.APIEndPoint + "Products"))
            //    {
            //        if (res.IsSuccessStatusCode)
            //        {
            //            var content = await res.Content.ReadAsStringAsync();
            //            var result = JsonConvert.DeserializeObject<BusinessResult>(content);

            //            if (result != null && result.Data != null)
            //            {
            //                var data = JsonConvert.DeserializeObject<List<Product>>(result.Data.ToString());

            //                return View(data);
            //            }
            //        }
            //    }
            //}
            //return View(new List<Product>());

            return View();
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var res = await httpClient.GetAsync(Const.APIEndPoint + $"Products/{id}"))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        var content = await res.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                        if (result != null && result.Data != null)
                        {
                            var data = JsonConvert.DeserializeObject<Product>(result.Data.ToString());

                            return View(data);
                        }
                    }
                }
            }
            return View(new Product());
        }

        // GET: Products/Create
        public async Task<IActionResult> Create()
        {
            var categories = await GetCategories();
            var staffs = await GetStaffs();

            ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "Name");
            ViewData["CreatedBy"] = new SelectList(staffs, "StaffId", "FullName");
            ViewData["ModifiedBy"] = new SelectList(staffs, "StaffId", "FullName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Bind("ProductId,Name,Description,Price,StockQuantity,Brand,Weight,Discount,ExpiryDate,ManufacturingDate,CategoryId,Status,CreatedAt,UpdatedAt,CreatedBy,ModifiedBy")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductReqModel product)
        {
            bool saveStatus = false;

            var categories = await GetCategories();
            var staffs = await GetStaffs();

            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.PostAsJsonAsync(Const.APIEndPoint + "Products", product))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                            if (result != null && result.Status == Const.SUCCESS_CREATE_CODE)
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
                    return RedirectToAction((nameof(Index)));
                }
                else
                {
                    ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "Name");
                    ViewData["CreatedBy"] = new SelectList(staffs, "StaffId", "FullName", product.CreatedBy);
                    return View(product);
                }
            }
            ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "Name");
            ViewData["CreatedBy"] = new SelectList(staffs, "StaffId", "FullName", product.CreatedBy);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var product = new Product();
            var categories = await GetCategories();
            var staffs = await GetStaffs();

            var updateProductModel = new UpdateProductReqModel();

            using (var httpClient = new HttpClient())
            {
                using (var res = await httpClient.GetAsync(Const.APIEndPoint + $"Products/{id}"))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        var content = await res.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                        if (result != null && result.Data != null)
                        {
                            product = JsonConvert.DeserializeObject<Product>(result.Data.ToString());

                            updateProductModel.ProductId = product.ProductId;
                            updateProductModel.Name = product.Name;
                            updateProductModel.Brand = product.Brand;
                            updateProductModel.Description = product.Description;
                            updateProductModel.Price = product.Price;
                            updateProductModel.StockQuantity = product.StockQuantity;
                            updateProductModel.Weight = product.Weight;
                            updateProductModel.Discount = product.Discount;
                            updateProductModel.ExpiryDate = product.ExpiryDate;
                            updateProductModel.ManufacturingDate = product.ManufacturingDate;
                            updateProductModel.CategoryId = product.CategoryId;
                            updateProductModel.ModifiedBy = product.ModifiedBy;
                            updateProductModel.Images = product.ProductImages.Select(x => x.ImageUrl).ToList();
                        }
                    }
                }
            }
            ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "Name", updateProductModel.CategoryId);
            ViewData["ModifiedBy"] = new SelectList(staffs, "StaffId", "FullName", updateProductModel.ModifiedBy);

            return View(updateProductModel);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Bind("ProductId,Name,Description,Price,StockQuantity,Brand,Weight,Discount,ExpiryDate,ManufacturingDate,CategoryId,Status,CreatedAt,UpdatedAt,CreatedBy,ModifiedBy")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  UpdateProductReqModel updateProductReqModel)
        {
            bool saveStatus = false;

            var categories = await GetCategories();
            var staffs = await GetStaffs();

            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.PutAsJsonAsync(Const.APIEndPoint + "Products", updateProductReqModel))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                            if (result != null && result.Status == Const.SUCCESS_UPDATE_CODE)
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
                    return RedirectToAction((nameof(Index)));
                }
                else
                {
                    ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "Name", updateProductReqModel.CategoryId);
                    ViewData["ModifiedBy"] = new SelectList(staffs, "StaffId", "FullName", updateProductReqModel.ModifiedBy);
                    return View(updateProductReqModel);
                }
            }

            ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "Name", updateProductReqModel.CategoryId);
            ViewData["ModifiedBy"] = new SelectList(staffs, "StaffId", "FullName", updateProductReqModel.ModifiedBy);
            return View(updateProductReqModel);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var product = new Product();
            var categories = new List<Category>();

            using (var httpClient = new HttpClient())
            {
                using (var res = await httpClient.GetAsync(Const.APIEndPoint + $"Products/{id}"))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        var content = await res.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                        if (result != null && result.Data != null)
                        {
                            product = JsonConvert.DeserializeObject<Product>(result.Data.ToString());

                        }
                    }
                }
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool saveStatus = false;
            var product = new Product();
            using (var httpClient = new HttpClient())
            {
                using (var res = await httpClient.DeleteAsync(Const.APIEndPoint + $"Products/{id}"))
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

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
