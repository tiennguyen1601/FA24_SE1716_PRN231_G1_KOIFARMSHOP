using KOIFARMSHOP.Common;
using KOIFARMSHOP.Data.Enums;
using KOIFARMSHOP.Data.Models;
using KOIFARMSHOP.Service.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace KOIFARMSHOP.MVCWebApp.Controllers
{
    public class PaymentAjaxController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var customers = await GetCustomers();
            var orders = await GetOrders();

            ViewData["CustomerId"] = new SelectList(customers, "CustomerId", "Name");
            ViewData["OrderId"] = new SelectList(orders, "OrderId", "OrderId");
            ViewData["Orders"] = orders;
            ViewData["Status"] = new SelectList(Enum.GetNames(typeof(PaymentEnums)).ToList());

            return View();
        }

        private async Task<List<Customer>> GetCustomers()
        {
            var list = new List<Customer>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.APIEndPoint + $"Customers"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                        if (result != null && result.Data != null)
                        {
                            list = JsonConvert.DeserializeObject<List<Customer>>(result.Data.ToString());
                        }
                    }
                };
            }

            return list;
        }

        private async Task<List<Order>> GetOrders()
        {
            var list = new List<Order>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.APIEndPoint + $"Orders/get-Order-by-user"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                        if (result != null && result.Data != null)
                        {
                            list = JsonConvert.DeserializeObject<List<Order>>(result.Data.ToString());
                        }
                    }
                };
            }

            return list;
        }
    }
}
