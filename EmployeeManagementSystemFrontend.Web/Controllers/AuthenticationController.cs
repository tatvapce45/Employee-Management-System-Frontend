using EmployeeManagementSystemFrontend.Web.Common;
using EmployeeManagementSystemFrontend.Web.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
namespace EmployeeManagementSystemFrontend.Web.Controllers
{
    [AllowAnonymous]
    public class AuthenticationController : Controller
    {
        private readonly HttpClient _httpClient;

        public AuthenticationController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("EmployeeManagementApi");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegistrationDto userRegistrationDto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var content = new StringContent(JsonConvert.SerializeObject(userRegistrationDto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/authentication/Register", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<UserRegistrationDto>>(responseContent);
            if (response.IsSuccessStatusCode && apiResponse != null && apiResponse.Success)
            {

                return RedirectToAction("Login", "Authentication");
            }

            ViewBag.Error = apiResponse?.Message ?? "Login failed.";
            ViewBag.Errors = apiResponse?.ValidationErrors;
            return View();
        }

        public async Task<IActionResult> Login()
        {
            string? token = HttpContext.Session.GetString("AccessToken") ?? Request.Cookies["AccessToken"];

            if (!string.IsNullOrEmpty(token))
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, "api/authentication/Validate-Token");
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Error = $"Error: {response.StatusCode}. Please check your authentication.";
                    return View();
                }
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var content = new StringContent(JsonConvert.SerializeObject(loginDto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("http://localhost:5287/api/authentication/Login", content);

            var responseContent = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<UserLoginDto>>(responseContent);

            if (response.IsSuccessStatusCode && apiResponse != null && apiResponse.Success)
            {

                HttpContext.Session.SetString("RememberMe", loginDto.RememberMe.ToString());
                return RedirectToAction("VerifyOtp", "Authentication", new { email = loginDto.Email });
            }

            ViewBag.Error = apiResponse?.Message ?? "Login failed.";
            ViewBag.Errors = apiResponse?.ValidationErrors;
            return View();
        }

        [HttpGet]
        public IActionResult VerifyOtp(string email)
        {
            return View(model: email);
        }

        [HttpPost]
        public async Task<IActionResult> VerifyOtp(string email, string otp)
        {
            var response = await _httpClient.PostAsync($"http://localhost:5287/api/authentication/Verify-OTP?email={email}&otp={otp}", null);
            var content = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<TokensDto>>(content);

            if (response.IsSuccessStatusCode && apiResponse?.Success == true)
            {
                string accessToken = apiResponse.Data!.AccessToken;
                string refreshToken = apiResponse.Data!.RefreshToken;

                _ = bool.TryParse(HttpContext.Session.GetString("RememberMe"), out bool rememberMe);

                if (rememberMe)
                {
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        Expires = DateTime.UtcNow.AddDays(30),
                        SameSite = SameSiteMode.Strict
                    };
                    Response.Cookies.Append("AccessToken", accessToken, cookieOptions);
                    Response.Cookies.Append("RefreshToken", refreshToken, cookieOptions);
                }
                else
                {
                    HttpContext.Session.SetString("AccessToken", accessToken);
                    HttpContext.Session.SetString("RefreshToken", refreshToken);
                }
                TempData["toastMessage"] = apiResponse.Message;
                TempData["messageType"] = apiResponse.Success == true ? "success" : "error";

                HttpContext.Session.Remove("RememberMe");
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

        [HttpGet]
        public async Task<JsonResult> GetRoles()
        {
            var response = await _httpClient.GetAsync("api/authentication/GetRoles");
            var responseString = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<RolesResponseDto>>(responseString);
            if (apiResponse is not null && apiResponse.Success && apiResponse.Data is not null)
            {
                return Json(apiResponse.Data.RolesDtos);
            }
            return Json("No roles found.");
        }

        [HttpGet]
        public async Task<JsonResult> GetCountries()
        {
            var response = await _httpClient.GetAsync("api/authentication/GetCountries");
            if (!response.IsSuccessStatusCode)
            {
                return Json(new { success = false, message = "Failed to retrieve countries." });
            }
            var responseString = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<CountriesResponseDto>>(responseString);
            if (apiResponse is not null && apiResponse.Success && apiResponse.Data is not null)
            {
                return Json(apiResponse.Data.CountriesDtos);
            }
            return Json(new { success = false, message = "No countries found." });
        }

        [HttpGet]
        public async Task<JsonResult> GetStatesByCountryId(int countryId)
        {
            var response = await _httpClient.GetAsync($"api/authentication/GetStatesbyCountryId?countryId={countryId}");
            if (!response.IsSuccessStatusCode)
            {
                return Json(new { success = false, message = "Failed to retrieve states." });
            }
            var responseString = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<StatesResponseDto>>(responseString);

            if (apiResponse?.Success == true && apiResponse.Data?.StatesDtos != null)
            {
                return Json(apiResponse.Data.StatesDtos);
            }

            return Json(new { success = false, message = "No states found." });
        }

        [HttpGet]
        public async Task<JsonResult> GetCitiesByStateId(int stateId)
        {
            var response = await _httpClient.GetAsync($"api/authentication/GetCitiesByStateId?stateId={stateId}");
            if (!response.IsSuccessStatusCode)
            {
                return Json(new { success = false, message = "Failed to retrieve cities." });
            }
            var responseString = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<CitiesResponseDto>>(responseString);
            if (apiResponse is not null && apiResponse.Success && apiResponse.Data is not null)
            {
                return Json(apiResponse.Data.CitiesDtos);
            }
            return Json(new { success = false, message = "No cities found." });
        }
    }
}
