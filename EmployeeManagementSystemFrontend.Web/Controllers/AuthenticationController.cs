using EmployeeManagementSystemFrontend.Web.Common;
using EmployeeManagementSystemFrontend.Web.Dtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace EmployeeManagementSystemFrontend.Web.Controllers
{
    public class AuthenticationController(IHttpClientFactory httpClientFactory) : Controller
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient("EmployeeManagementApi");

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto dto)
        {
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/authentication/Register", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return RedirectToAction("Login");
            }

            var error = await response.Content.ReadAsStringAsync();
            ViewBag.Error = error;
            return View();
        }

        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDto loginDto)
        {
            var content = new StringContent(JsonConvert.SerializeObject(loginDto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("http://localhost:5287/api/authentication/Login", content);

            var responseContent = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<UserLoginDto>>(responseContent);

            if (response.IsSuccessStatusCode && apiResponse != null && apiResponse.Success)
            {
                ViewBag.Message = apiResponse.Message;
                return RedirectToAction("VerifyOtp", "Authentication");
            }

            ViewBag.Error = apiResponse?.Message ?? "Login failed.";
            ViewBag.Errors = apiResponse?.ValidationErrors;
            return View();
        }

        [HttpGet]
        public IActionResult VerifyOtp()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> VerifyOtp(string email, string otp)
        {
            var response = await _httpClient.PostAsync($"http://localhost:5287/api/authentication/Verify-OTP?email={email}&otp={otp}", null);
            var content = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<object>>(content);

            if (response.IsSuccessStatusCode && apiResponse?.Success == true)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = apiResponse?.Message;
            ViewBag.Errors = apiResponse?.ValidationErrors;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRefreshRequestDto refreshRequestDto)
        {
            var content = new StringContent(JsonConvert.SerializeObject(refreshRequestDto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/authentication/Refresh-Token", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonConvert.DeserializeObject<dynamic>(result);
                string newAccessToken = tokenResponse!.AccessToken;

                HttpContext.Session.SetString("AccessToken", newAccessToken);
                return Ok(new { AccessToken = newAccessToken });
            }

            return Unauthorized("Failed to refresh token.");
        }
    }
}
