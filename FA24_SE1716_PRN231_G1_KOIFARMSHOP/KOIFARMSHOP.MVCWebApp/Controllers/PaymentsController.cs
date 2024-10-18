using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KOIFARMSHOP.Service;
using Newtonsoft.Json;
using KOIFARMSHOP.Common;
using KOIFARMSHOP.Service.Base;
using KOIFARMSHOP.Data.Models;
using KOIFARMSHOP.Data.DTO.PaymentDTO;
using KOIFARMSHOP.Data.Enums;
using System.Net;
using Microsoft.AspNetCore.Mvc.Filters;

namespace KOIFARMSHOP.MVCWebApp.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly HttpClient _httpClient;

        public override void OnActionExecuting(ActionExecutingContext ctx)
        {
            base.OnActionExecuting(ctx);

            var token = HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public PaymentsController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(Const.APIEndPoint);            
        }

        // GET: Payments
        public async Task<IActionResult> Index(
            string? method, 
            string? status, 
            string? transactionId, 
            DateTime? paymentDate, 
            DateTime? createdAt, 
            DateTime? updatedAt, 
            string? customerName, 
            string? orderId, 
            decimal? minAmount,
            decimal? maxAmount)
        {
            var queryParameters = new List<string>();
            queryParameters.Add($"method={Uri.EscapeDataString(method ?? "")}");
            queryParameters.Add($"status={Uri.EscapeDataString(status ?? "")}");
            queryParameters.Add($"transactionId={Uri.EscapeDataString(transactionId ?? "")}");
            if (paymentDate.HasValue)
                queryParameters.Add($"paymentDate={paymentDate.Value:yyyy-MM-dd}");
            if (createdAt.HasValue)
                queryParameters.Add($"createdAt={createdAt.Value:yyyy-MM-dd}");
            if (updatedAt.HasValue)
                queryParameters.Add($"updatedAt={updatedAt.Value:yyyy-MM-dd}");
            queryParameters.Add($"customerName={Uri.EscapeDataString(customerName ?? "")}");
            queryParameters.Add($"orderId={Uri.EscapeDataString(orderId ?? "")}");
            if (minAmount.HasValue)
                queryParameters.Add($"minAmount={minAmount.Value}");
            if (maxAmount.HasValue)
                queryParameters.Add($"maxAmount={maxAmount.Value}");

            var queryString = string.Join("&", queryParameters);

            using (var response = await _httpClient.GetAsync($"Payments?{queryString}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                    if (result != null && result.Data != null)
                    {
                        var data = JsonConvert.DeserializeObject<List<Payment>>(result.Data.ToString());

                        ViewBag.Method = method;
                        ViewBag.Status = status;
                        ViewBag.TransactionId = transactionId;
                        ViewBag.PaymentDate = paymentDate;
                        ViewBag.CreatedAt = createdAt;
                        ViewBag.UpdatedAt = updatedAt;
                        ViewBag.CustomerName = customerName;
                        ViewBag.OrderId = orderId;
                        ViewBag.MinAmount = minAmount;
                        ViewBag.MaxAmount = maxAmount;

                        return View(data);
                    }
                }else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Redirect("/Customers/Login");
                }
            }
            return View(new List<Payment>());
        }

        // GET: Payments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            using (var response = await _httpClient.GetAsync($"Payments/{id}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                    if (result != null && result.Data != null)
                    {
                        var data = JsonConvert.DeserializeObject<Payment>(result.Data.ToString());
                        return View(data);
                    }
                }
            }
            return View();
        }

        // GET: Payments/Create
        public async Task<IActionResult> Create()
        {
            var customers = await GetCustomers();
            var orders = await GetOrders();

            ViewData["CustomerId"] = new SelectList(customers, "CustomerId", "Name");
            ViewData["OrderId"] = new SelectList(orders, "OrderId", "OrderId");
            ViewData["Orders"] = orders;
            return View();
        }

        // POST: Payments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PaymentId,OrderId,Method")] PaymentCreateRequestModel payment)
        {
            bool saveStatus = false;

            if (ModelState.IsValid)
            {
                using (var response = await _httpClient.PostAsJsonAsync($"Payments", payment))
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
                return RedirectToAction(nameof(Index));
            }

            ViewData["CustomerId"] = new SelectList(await GetCustomers(), "CustomerId", "Username");
            ViewData["OrderId"] = new SelectList(await GetOrders(), "OrderId", "OrderId");
            return View(payment);
        }

        // GET: Payments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var payment = new Payment();

            using (var response = await _httpClient.GetAsync($"Payments/{id}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                    if (result != null && result.Data != null)
                    {
                        payment = JsonConvert.DeserializeObject<Payment>(result.Data.ToString());
                    }
                }
            }

            if (payment == null)
            {
                return NotFound();
            }

            ViewData["Status"] = new SelectList(Enum.GetNames(typeof(PaymentEnums)).ToList());
            ViewData["CustomerId"] = new SelectList(await GetCustomers(), "CustomerId", "Name", payment.CustomerId);
            ViewData["OrderId"] = new SelectList(await GetOrders(), "OrderId", "OrderId", payment.OrderId);
            return View(payment);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PaymentId,OrderId,Method,Status,TransactionId")] PaymentCreateRequestModel payment)
        {
            bool saveStatus = false;

            if (ModelState.IsValid)
            {
                using (var response = await _httpClient.PutAsJsonAsync($"Payments", payment))
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
                return RedirectToAction(nameof(Index));
            }

            ViewData["CustomerId"] = new SelectList(await GetCustomers(), "CustomerId", "Name");
            ViewData["OrderId"] = new SelectList(await GetOrders(), "OrderId", "OrderId");
            return View(payment);
        }

        // GET: Payments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var payment = new Payment();

            using (var response = await _httpClient.GetAsync($"Payments/{id}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                    if (result != null && result.Data != null)
                    {
                        payment = JsonConvert.DeserializeObject<Payment>(result.Data.ToString());
                    }
                }
            }

            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool saveStatus = false;

            if (ModelState.IsValid)
            {
                using (var response = await _httpClient.DeleteAsync($"Payments/{id}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
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

            return View();
        }

        private async Task<List<Customer>> GetCustomers() {
            var list = new List<Customer>();

            using (var response = await _httpClient.GetAsync($"Customers"))
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

            return list;
        }

        private async Task<List<Order>> GetOrders()
        {
            var list = new List<Order>();

            using (var response = await _httpClient.GetAsync($"Orders"))
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

            return list;
        }
    }
}
