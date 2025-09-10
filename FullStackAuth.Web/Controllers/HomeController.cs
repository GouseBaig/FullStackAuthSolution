using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;




namespace FullStackAuth.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var client = _httpClientFactory.CreateClient("AuthAPI");

            var loginData = new
            {
                username = username,
                password = password
            };

            var json = JsonSerializer.Serialize(loginData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync("auth/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent);

                    // Store token in session
                    HttpContext.Session.SetString("token", tokenResponse.token);

                    // Get user profile
                    return await GetUserProfile();
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(errorContent);
                    ViewBag.Error = errorResponse.error;
                    return View("Index");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Connection error: " + ex.Message;
                return View("Index");
            }
        }

        public async Task<IActionResult> Profile()
        {
            return await GetUserProfile();
        }

        private async Task<IActionResult> GetUserProfile()
        {
            var token = HttpContext.Session.GetString("token");

            if (string.IsNullOrEmpty(token))
            {
                ViewBag.Error = "Please login first";
                return View("Index");
            }

            var client = _httpClientFactory.CreateClient("AuthAPI");
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            try
            {
                var response = await client.GetAsync("auth/profile");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var userProfile = JsonSerializer.Deserialize<UserProfile>(responseContent);
                    return View("Profile", userProfile);
                }
                else
                {
                    ViewBag.Error = "Failed to get user profile";
                    return View("Index");
                }
            }
            catch (Exception ex)
            {
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

    public class TokenResponse
    {
        public string token { get; set; } = string.Empty;
    }

    public class ErrorResponse
    {
        public string error { get; set; } = string.Empty;
    }

    public class UserProfile
    {
        public int id { get; set; }
        public string username { get; set; } = string.Empty;
        public string role { get; set; } = string.Empty;
    }
}
