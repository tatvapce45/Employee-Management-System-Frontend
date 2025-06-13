using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagementSystemFrontend.Web.Models;
using EmployeeManagementSystemFrontend.Web.Dtos;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http;
using EmployeeManagementSystemFrontend.Web.Common;

namespace EmployeeManagementSystemFrontend.Web.Controllers;

public class HomeController(IHttpClientFactory httpClientFactory) : Controller
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("EmployeeManagementApi");

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var requestDto = new EmployeesRequestDto
        {
            DepartmentId = 1
        };

        var content = new StringContent(JsonConvert.SerializeObject(requestDto), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("api/employee/GetEmployees", content);
        var responseString = await response.Content.ReadAsStringAsync();

        var apiResponse = JsonConvert.DeserializeObject<ApiResponse<EmployeesResponseDto>>(responseString);

        if (apiResponse is not null && apiResponse.Success)
        {
            return View(apiResponse.Data);
        }

        ViewBag.Error = apiResponse?.Message ?? "Failed to fetch employees.";
        return View(new List<EmployeeDto>());
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
