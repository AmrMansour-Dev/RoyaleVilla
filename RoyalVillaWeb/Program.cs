using Microsoft.AspNetCore.Authentication.Cookies;
using RoyalVilla.DTO;
using RoyalVillaWeb.Services;
using RoyalVillaWeb.Services.IServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(o =>
{
    o.IdleTimeout = TimeSpan.FromMinutes(60);
    o.Cookie.HttpOnly = true;
    o.Cookie.IsEssential = true;
});

builder.Services.AddAutoMapper(options => // we used this to map between DTOs and Models
{
    options.CreateMap<VillaCreateDTO, VillaDTO>();
    options.CreateMap<VillaUpdateDTO, VillaDTO>();
    options.CreateMap<VillaDTO, VillaUpdateDTO>();

});

builder.Services.AddHttpClient("RoualVillaAPI", Client =>
{
    var VillaAPIurl = builder.Configuration.GetValue<string>("ServiceUrls:VillaAPI");
    Client.BaseAddress = new Uri(VillaAPIurl);
    Client.DefaultRequestHeaders.Add("Accept", "application/json");
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;
    options.LoginPath = "/Auth/Login";
    options.AccessDeniedPath = "/Auth/accessdenied";
});

builder.Services.AddScoped<IVillaService, VillaService>();
builder.Services.AddScoped<IAuthService, AuthService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();

app.UseRouting();

app.UseSession();

app.UseAuthentication();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
