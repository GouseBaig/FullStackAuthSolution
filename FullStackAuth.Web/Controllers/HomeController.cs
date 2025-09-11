using FullStackAuth.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace FullStackAuth.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApiSettings _apiSettings;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            IHttpClientFactory httpClientFactory,
            IOptions<ApiSettings> apiSettings,
            ILogger<HomeController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _apiSettings = apiSettings.Value;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var client = _httpClientFactory.CreateClient("Api");


            var loginData = new { username, password };
            var content = new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync(_apiSettings.Endpoints.Login, content);
                var resContent = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Response from {Endpoint}: {Content}", _apiSettings.Endpoints.Login, resContent);
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(errorContent);
                    ViewBag.Error = errorResponse?.error ?? "Login failed";
                    return View("Index");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent);

                if (string.IsNullOrEmpty(tokenResponse?.token))
                {
                    ViewBag.Error = "Invalid token received from server.";
                    return View("Index");
                }

                HttpContext.Session.SetString("token", tokenResponse.token);

                return RedirectToAction("Profile");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login API call");
                ViewBag.Error = "Connection error: " + ex.Message;
                return View("Index");
            }
        }

        public async Task<IActionResult> Profile()
        {
            var token = HttpContext.Session.GetString("token");

            if (string.IsNullOrEmpty(token))
            {
                ViewBag.Error = "Please login first";
                return View("Index");
            }

            var client = _httpClientFactory.CreateClient("Api");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var response = await client.GetAsync(_apiSettings.Endpoints.Profile);

                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.Error = "Failed to get user profile";
                    return View("Index");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var userProfile = JsonSerializer.Deserialize<UserProfile>(responseContent);

                return View("Profile", userProfile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during profile API call");
                ViewBag.Error = "Connection error: " + ex.Message;
                return View("Index");
            }
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("token");
            return RedirectToAction("Index");
        }
    }
}
