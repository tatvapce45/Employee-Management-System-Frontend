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
                return PartialView("_EmployeesList", apiResponse.Data);
            }

            ViewBag.Error = apiResponse?.Message ?? "Failed to fetch employees.";
            return PartialView("_EmployeesList", new EmployeesResponseDto());
        }

        public IActionResult GetAddDepartmentModal()
        {
            return PartialView("_AddDepartment");
        }

        public async Task<IActionResult> AddDepartment(CreateDepartmentDto createDepartmentDto)
        {
            var content = new StringContent(JsonConvert.SerializeObject(createDepartmentDto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/employee/CreateDepartment", content);
            var responseString = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<DepartmrentDto>>(responseString);

            if (apiResponse is not null && apiResponse.Success)
            {
                return Json(new { success = true, messageType = "success", toastMessage = apiResponse.Message });
            }
            return Json(new { success = false, messageType = "error", toastMessage = apiResponse?.Message ?? "Unknown error." });
        }

        public async Task<IActionResult> GetUpdateDepartmentModal(int id)
        {
            var response = await _httpClient.GetAsync($"api/employee/GetDepartmentById?departmentId={id}");
            var responseString = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<DepartmrentDto>>(responseString);

            if (apiResponse is not null && apiResponse.Success)
            {
                DepartmrentDto departmrentDto = apiResponse.Data!;
                return PartialView("_UpdateDepartment", departmrentDto);
            }
            return PartialView("_UpdateDepartment");
        }

        public async Task<IActionResult> UpdateDepartment(UpdateDepartmentDto updateDepartmentDto)
        {
            var content = new StringContent(JsonConvert.SerializeObject(updateDepartmentDto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PatchAsync("api/employee/UpdateDepartment", content);
            var responseString = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<DepartmrentDto>>(responseString);

            if (apiResponse is not null && apiResponse.Success)
            {
                return Json(new { success = true, messageType = "success", toastMessage = apiResponse.Message });
            }
            return Json(new { success = false, messageType = "error", toastMessage = apiResponse?.Message ?? "Unknown error." });
        }

        public IActionResult GetConfirmationModal(string confirmationMessage, string confirmationWork, int? id)
        {
            ConfirmationModalDto confirmationModalDto = new()
            {
                ConfirmationMessage = confirmationMessage,
                ConfirmationWork = confirmationWork,
                Id = id ?? 0
            };
            return PartialView("_ConfirmationModal", confirmationModalDto);
        }

        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/employee/DeleteDepartment?departmentId={id}");
            var responseString = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<string>>(responseString);

            if (apiResponse is not null && apiResponse.Success)
            {
                return Json(new { success = true, messageType = "success", toastMessage = apiResponse.Message });
            }
            return Json(new { success = false, messageType = "error", toastMessage = apiResponse?.Message ?? "Unknown error." });
        }

        public IActionResult GetAddEmployeeModal()
        {
            return PartialView("_AddEmployee");
        }

        [HttpGet]
        public async Task<JsonResult> GetDepartmentsJson()
        {
            var response = await _httpClient.GetAsync("api/employee/GetDepartments");
            if (!response.IsSuccessStatusCode)
            {
                return Json(new { success = false, message = "Failed to retrieve countries." });
            }
            var responseString = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<DepartmentsDto>>(responseString);
            if (apiResponse is not null && apiResponse.Success && apiResponse.Data is not null)
            {
                return Json(apiResponse.Data.Departments);
            }
            return Json(new { success = false, message = "No departments found." });
        }

        public async Task<IActionResult> AddEmployee(CreateEmployeeDto createEmployeeDto)
        {
            createEmployeeDto.RoleId = 3;
            var content = new StringContent(JsonConvert.SerializeObject(createEmployeeDto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/employee/CreateEmployee", content);
            var responseString = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<EmployeeDto>>(responseString);

            if (apiResponse is not null && apiResponse.Success)
            {
                return Json(new { success = true, messageType = "success", toastMessage = apiResponse.Message });
            }
            return Json(new { success = false, messageType = "error", toastMessage = apiResponse?.Message ?? "Unknown error." });
        }

        public async Task<IActionResult> GetUpdateEmployeeModal(int id)
        {
            var response = await _httpClient.GetAsync($"api/employee/GetEmployeeById?employeeId={id}");
            var responseString = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<EmployeeDto>>(responseString);

            if (apiResponse is not null && apiResponse.Success)
            {
                EmployeeDto employeeDto = apiResponse.Data!;
                return PartialView("_UpdateEmployee", employeeDto);
            }
            return PartialView("_UpdateEmployee");
        }

        public async Task<IActionResult> UpdateEmployee(UpdateEmployeeDto updateEmployeeDto)
        {
            updateEmployeeDto.RoleId = 3;
            var content = new StringContent(JsonConvert.SerializeObject(updateEmployeeDto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PatchAsync("api/employee/UpdateEmployee", content);
            var responseString = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<EmployeeDto>>(responseString);

            if (apiResponse is not null && apiResponse.Success)
            {
                return Json(new { success = true, messageType = "success", toastMessage = apiResponse.Message });
            }
            return Json(new { success = false, messageType = "error", toastMessage = apiResponse?.Message ?? "Unknown error." });
        }

        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/employee/DeleteEmployee?employeeId={id}");
            var responseString = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<string>>(responseString);

            if (apiResponse is not null && apiResponse.Success)
            {
                return Json(new { success = true, messageType = "success", toastMessage = apiResponse.Message });
            }
            return Json(new { success = false, messageType = "error", toastMessage = apiResponse?.Message ?? "Unknown error." });
        }

        public async Task<IActionResult> DeleteMultipleEmployees(int[] selectedEmployees)
        {
            var content = new StringContent(JsonConvert.SerializeObject(selectedEmployees), Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(_httpClient.BaseAddress + "api/employee/DeleteMultipleEmployees"),
                Content = content
            };

            var response = await _httpClient.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<string>>(responseString);

            if (apiResponse is not null && apiResponse.Success)
            {
                return Json(new { success = true, messageType = "success", toastMessage = apiResponse.Message });
            }

            return Json(new { success = false, messageType = "error", toastMessage = apiResponse?.Message ?? "Unknown error." });
        }

    }
}