using RoyalVilla.DTO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddAutoMapper(options => // we used this to map between DTOs and Models
{
    options.CreateMap<VillaCreateDTO, VillaDTO>();
    options.CreateMap<VillaUpdateDTO, VillaDTO>();

});

builder.Services.AddHttpClient("RoualVillaAPI", Client =>
{
    var VillaAPIurl = builder.Configuration.GetValue<string>("ServiceUrls:VillaAPI");
    Client.BaseAddress = new Uri(VillaAPIurl);
    Client.DefaultRequestHeaders.Add("Accept", "application/json");
});

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

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
