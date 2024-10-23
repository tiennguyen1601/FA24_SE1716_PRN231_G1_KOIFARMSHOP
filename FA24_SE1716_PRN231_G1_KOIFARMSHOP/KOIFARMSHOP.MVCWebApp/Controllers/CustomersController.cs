using KOIFARMSHOP.Data.DTO.LoginDTO;
using System.Net.Http;
using System.Net.Http.Json; // For using PostAsJsonAsync
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KOIFARMSHOP.Data.Models;
using KOIFARMSHOP.Service.Base;
using KOIFARMSHOP.Common;
using Newtonsoft.Json;

namespace KOIFARMSHOP.MVCWebApp.Controllers
{
    public class CustomersController : Controller
    {
        private readonly FA24_SE1716_PRN231_G1_KOIFARMSHOPContext _context;
        private readonly HttpClient _httpClient;

        public CustomersController(FA24_SE1716_PRN231_G1_KOIFARMSHOPContext context)
        {
            _context = context;
            _httpClient = new HttpClient();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginReqModel loginReqModel)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PostAsJsonAsync(Const.APIEndPoint + "Authentication/login", loginReqModel);

                if (response.IsSuccessStatusCode)
                {

                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                    if (result != null && (result.Status == 4 || result.Status == -1))
                    {
                        ModelState.AddModelError("", "Thông tin đăng nhập không chính xác.");
                        return View();
                    }

                        if (result != null && result.Data != null)
                    {
                        var data = JsonConvert.DeserializeObject<
                            LoginResModel>(result.Data.ToString());

                        HttpContext.Session.SetString("Token", data.Token); 
                        HttpContext.Session.SetString("Username", data.Username);
                        HttpContext.Session.SetString("Role", data.Role);

                        HttpContext.Session.SetString("UserId", data.UserId.ToString());
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Thông tin đăng nhập không chính xác.");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Lỗi khi gọi dịch vụ xác thực.");
                }
            }

            return View(loginReqModel);
        }
    }
}
