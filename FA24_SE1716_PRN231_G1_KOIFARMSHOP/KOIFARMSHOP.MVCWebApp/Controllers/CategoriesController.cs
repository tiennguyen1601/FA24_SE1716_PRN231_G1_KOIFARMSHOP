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
    }
}
