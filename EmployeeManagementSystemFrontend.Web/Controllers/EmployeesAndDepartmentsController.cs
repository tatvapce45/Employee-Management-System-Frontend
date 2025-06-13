using EmployeeManagementSystemFrontend.Web.Common;
using EmployeeManagementSystemFrontend.Web.Dtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EmployeeManagementSystemFrontend.Web.Controllers
{
    public class EmployeesAndDepartmentsController(IHttpClientFactory httpClientFactory) : Controller
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient("EmployeeManagementApi");

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetDepartmentsContainer()
        {
            return PartialView("_DepartmentsListDiv");
        }

        [HttpGet]
        public async Task<IActionResult> GetDepartments()
        {
            var response = await _httpClient.GetAsync("api/department/GetDepartments");
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse<List<DepartmentsDto>>>(responseString);
                if (apiResponse != null && apiResponse.Success)
                {
                    return PartialView("_DepartmentsList", apiResponse.Data);
                }
            }
            ViewBag.Error = "Failed to fetch departments.";
            return PartialView("_DepartmentsList", new DepartmentsDto());
        }
    }
}