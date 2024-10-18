using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KOIFARMSHOP.Data.Models;
using KOIFARMSHOP.Service.Services;
using KOIFARMSHOP.Common;
using KOIFARMSHOP.Service.Base;
using Newtonsoft.Json;
using KOIFARMSHOP.Data.DTO.CategoryDTO;
using KOIFARMSHOP.Data.DTO.ProductDTO;

namespace KOIFARMSHOP.MVCWebApp.Controllers
{
    public class CategoriesController : Controller
    {

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return View();
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var res = await httpClient.GetAsync(Const.APIEndPoint + $"Category/{id}"))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        var content = await res.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                        if (result != null && result.Data != null)
                        {
                            var data = JsonConvert.DeserializeObject<Category>(result.Data.ToString());

                            return View(data);
                        }
                    }
                }
            }
            return View(new Category());
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,Name,Description,Status")] CategoryCreateReqModel categoryCreateReqModel)
        {
            bool saveStatus = false;

            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.PostAsJsonAsync(Const.APIEndPoint + "Category", categoryCreateReqModel))
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
                
            }
            return View(categoryCreateReqModel);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var res = await httpClient.GetAsync(Const.APIEndPoint + $"Category/{id}"))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        var content = await res.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                        if (result != null && result.Data != null)
                        {
                            var data = JsonConvert.DeserializeObject<Category>(result.Data.ToString());

                            return View(data);
                        }
                    }
                }
            }
            return View(new Category());
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,Name,Description,Status")] CategoryCreateReqModel category)
        {
            bool saveStatus = false;

            var categories = new List<Category>();

            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.PutAsJsonAsync(Const.APIEndPoint + "Products", category))
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
                    
                    return View(category);
                }
            }

            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var res = await httpClient.GetAsync(Const.APIEndPoint + $"Category/{id}"))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        var content = await res.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                        if (result != null && result.Data != null)
                        {
                            var data = JsonConvert.DeserializeObject<Category>(result.Data.ToString());

                            return View(data);
                        }
                    }
                }
            }
            return View(new Category());
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool saveStatus = false;
            var product = new Category();
            using (var httpClient = new HttpClient())
            {
                using (var res = await httpClient.DeleteAsync(Const.APIEndPoint + $"Category/{id}"))
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

        //private bool CategoryExists(int id)
        //{
        //    return _context.Categories.Any(e => e.CategoryId == id);
        //}
    }
}
