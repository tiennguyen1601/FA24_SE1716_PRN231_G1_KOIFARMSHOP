using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using KOIFARMSHOP.Data.Models;
using KOIFARMSHOP.Common;
using KOIFARMSHOP.Service.Base;
using Attribute = KOIFARMSHOP.Data.Models.Attribute;

namespace KOIFARMSHOP.MVCWebApp.Controllers
{
    public class AttributesController : Controller
    {
        // Hiển thị danh sách Attribute
        public async Task<IActionResult> Index()
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"{Const.APIEndPoint}Attribute");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                if (result != null && result.Data != null)
                {
                    var attributes = JsonConvert.DeserializeObject<List<Attribute>>(result.Data.ToString());
                    return View(attributes);
                }
            }
            return View(new List<Attribute>());
        }

        // Hiển thị chi tiết Attribute
        public async Task<IActionResult> Details(int id)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"{Const.APIEndPoint}Attribute/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                if (result != null && result.Data != null)
                {
                    var attribute = JsonConvert.DeserializeObject<Attribute>(result.Data.ToString());
                    return View(attribute);
                }
            }
            return NotFound();
        }

        // Tạo Attribute mới
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Attribute attribute)
        {
            if (ModelState.IsValid)
            {
                using var httpClient = new HttpClient();
                var response = await httpClient.PostAsJsonAsync($"{Const.APIEndPoint}Attribute", attribute);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(attribute);
        }

        // Sửa Attribute
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"{Const.APIEndPoint}Attribute/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                if (result != null && result.Data != null)
                {
                    var attribute = JsonConvert.DeserializeObject<Attribute>(result.Data.ToString());
                    return View(attribute);
                }
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Attribute attribute)
        {
            if (ModelState.IsValid)
            {
                using var httpClient = new HttpClient();
                var response = await httpClient.PutAsJsonAsync($"{Const.APIEndPoint}Attribute/{id}", attribute);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(attribute);
        }

        // Xóa Attribute
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"{Const.APIEndPoint}Attribute/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                if (result != null && result.Data != null)
                {
                    var attribute = JsonConvert.DeserializeObject<Attribute>(result.Data.ToString());
                    return View(attribute);
                }
            }
            return NotFound();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.DeleteAsync($"{Const.APIEndPoint}Attribute/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
