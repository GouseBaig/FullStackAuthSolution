var builder = WebApplication.CreateBuilder(args);

// Add MVC controllers + Razor views
builder.Services.AddControllersWithViews();

// Configure HttpClient for API calls using BaseUrl from appsettings.json
var apiBaseUrl = builder.Configuration.GetValue<string>("ApiBaseUrl");
if (string.IsNullOrEmpty(apiBaseUrl))
{
    throw new InvalidOperationException("API base URL is not configured. Please check appsettings.json.");
}

builder.Services.AddHttpClient("Api", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

// Enable Session (in-memory store)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthorization();

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
