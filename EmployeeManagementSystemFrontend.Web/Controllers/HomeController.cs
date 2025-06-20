using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagementSystemFrontend.Web.Models;
using EmployeeManagementSystemFrontend.Web.Dtos;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http;
using EmployeeManagementSystemFrontend.Web.Common;
using Microsoft.AspNetCore.WebUtilities;

namespace EmployeeManagementSystemFrontend.Web.Controllers;

public class HomeController(IHttpClientFactory httpClientFactory, TokensHelper tokensHelper) : Controller
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("EmployeeManagementApi");
    private readonly TokensHelper _tokensHelper = tokensHelper;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View();
    }

    public async Task<JsonResult> GetDashboardData(int timeId=1, string fromDate = "", string toDate = "")
    {
        var queryParams = new Dictionary<string, string?>
        {
            ["timeId"] = timeId.ToString(),
            ["fromDate"] = fromDate,
            ["toDate"] = toDate,
        };
        var backendApiUrl = QueryHelpers.AddQueryString($"api/home/GetDashboardData", queryParams);
        var response = await _httpClient.GetAsync(backendApiUrl);
        var responseString = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonConvert.DeserializeObject<ApiResponse<DashboardResponseDto>>(responseString);
        return Json(apiResponse!.Data);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Logout()
    {
        TempData["toastMessage"] = "You have been logged out successfully";
        TempData["messageType"] = "success";
        _tokensHelper.ClearTokens();
        return Ok();
    }

    public async Task<IActionResult> MyProfile(int id)
    {
        var response = await _httpClient.GetAsync($"api/home/GetEmployeeToUpdateProfile?employeeId={id}");
        var responseString = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonConvert.DeserializeObject<ApiResponse<UpdateProfileDto>>(responseString);
        if (apiResponse is not null && apiResponse.Success)
        {
            return View(apiResponse.Data);
        }
        return BadRequest(apiResponse.ValidationErrors);
    }

    public async Task<IActionResult> UpdateProfile(UpdateProfileDto updateProfileDto)
    {
        var content = new StringContent(JsonConvert.SerializeObject(updateProfileDto), Encoding.UTF8, "application/json");
        var response = await _httpClient.PatchAsync("api/home/UpdateProfile", content);
        var responseString = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonConvert.DeserializeObject<ApiResponse<EmployeeDto>>(responseString);
        TempData["toastMessage"] = apiResponse.Message;
        TempData["messageType"] = apiResponse.Success == true ? "success" : "error";
        if (apiResponse is not null && apiResponse.Success)
        {
            return RedirectToAction("Index", "Home");
        }
        return View(updateProfileDto);
    }

    public IActionResult ChangePassword(int id)
    {
        ChangePasswordDto changePasswordDto = new()
        {
            Id = id
        };
        return View(changePasswordDto);
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
    {
        var content = new StringContent(JsonConvert.SerializeObject(changePasswordDto), Encoding.UTF8, "application/json");
        var response = await _httpClient.PatchAsync("api/home/ChangePassword", content);
        var responseString = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonConvert.DeserializeObject<ApiResponse<ChangePasswordDto>>(responseString);
        TempData["toastMessage"] = apiResponse.Message;
        TempData["messageType"] = apiResponse.Success == true ? "success" : "error";
        if (apiResponse is not null && apiResponse.Success)
        {
            return RedirectToAction("Index", "Home");
        }
        return View(changePasswordDto);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
