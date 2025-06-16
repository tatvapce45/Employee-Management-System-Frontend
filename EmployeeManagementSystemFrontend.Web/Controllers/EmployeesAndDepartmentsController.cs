using System.Text;
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
        public IActionResult GetDepartmentsContainer()
        {
            return PartialView("_DepartmentsListDiv");
        }

        [HttpGet]
        public async Task<IActionResult> GetDepartments()
        {
            var response = await _httpClient.GetAsync("api/Employee/GetDepartments");
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse<DepartmentsDto>>(responseString);
                if (apiResponse != null && apiResponse.Success)
                {
                    return PartialView("_DepartmentsList", apiResponse.Data);
                }
            }
            ViewBag.Error = "Failed to fetch departments.";
            return PartialView("_DepartmentsList", new DepartmentsDto());
        }

        [HttpGet]
        public IActionResult GetEmployeesContainer()
        {
            return PartialView("_EmployeesListDiv");
        }

        [HttpPost]
        public async Task<IActionResult> GetEmployees([FromBody] EmployeesRequestDto employeesRequestDto)
        {
            var content = new StringContent(JsonConvert.SerializeObject(employeesRequestDto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/employee/GetEmployees", content);
            var responseString = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<EmployeesResponseDto>>(responseString);

            if (apiResponse is not null && apiResponse.Success)
            {
                return PartialView("_EmployeesList",apiResponse.Data);
            }

            ViewBag.Error = apiResponse?.Message ?? "Failed to fetch employees.";
            return View(new List<EmployeeDto>());
        }

    }
}