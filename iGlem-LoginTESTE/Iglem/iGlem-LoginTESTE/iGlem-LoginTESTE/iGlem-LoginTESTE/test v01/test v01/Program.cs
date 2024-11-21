using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using test_v01.Repository;

var builder = WebApplication.CreateBuilder(args);

// Register session services
builder.Services.AddDistributedMemoryCache(); // In-memory cache provider for session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set the session timeout
    options.Cookie.HttpOnly = true; // Ensure the cookie is HTTP-only
    options.Cookie.IsEssential = true; // Mark the session cookie as essential
});


builder.Services.AddDbContext<SITEtccDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromDays(30); 
        options.SlidingExpiration = true; 
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.MaxAge = TimeSpan.FromDays(30); 
    });

builder.Services.AddControllersWithViews();
builder.Services.AddAuthorization();

var app = builder.Build();


app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=HomeList}/{action=Index}/{id?}");
});



app.Run();
