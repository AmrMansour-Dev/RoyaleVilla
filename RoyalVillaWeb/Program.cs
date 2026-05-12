using RoyalVilla.DTO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddAutoMapper(options => // we used this to map between DTOs and Models
{
    options.CreateMap<VillaCreateDTO, Villa>();
    options.CreateMap<VillaUpdateDTO, Villa>();
    options.CreateMap<Villa, VillaDTO>();
    options.CreateMap<VillaUpdateDTO, VillaDTO>();
    options.CreateMap<User, UserDTO>();
    options.CreateMap<VillaAmenitiesUpdateDTO, VillaAmenities>();
    options.CreateMap<VillaAmenitiesCreateDTO, VillaAmenities>();
    options.CreateMap<VillaAmenities, VillaAmenitiesDTO>().ForMember(dest => dest.VillaName, opt => opt.MapFrom(src => src.Villa != null ? src.Villa.Name : null));
    options.CreateMap<VillaAmenitiesDTO, VillaAmenities>();

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
