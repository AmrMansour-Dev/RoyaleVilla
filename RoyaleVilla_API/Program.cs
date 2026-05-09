using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using RoyaleVilla_API.Data;
using RoyaleVilla_API.Models;
using RoyaleVilla_API.Models.DTO;
using RoyaleVilla_API.Services;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JWT")["Secret"]);


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero

    };

});

builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Components ??= new OpenApiComponents();

        document.Components.SecuritySchemes = new Dictionary<string,IOpenApiSecurityScheme>
        {
            ["Bearer"] = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "Enter: Bearer {your token}"
            }

        };

        document.Security =
         [
            new OpenApiSecurityRequirement
           {
                {new OpenApiSecuritySchemeReference("Bearer"),new List<string>() }
           }
        ];

        return Task.CompletedTask;
    });
});

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddAutoMapper(options => // we used this to map between DTOs and Models
{
    options.CreateMap<VillaCreateDTO, Villa>();
    options.CreateMap<VillaUpdateDTO, Villa>();
    options.CreateMap<Villa, VillaDTO>();
    options.CreateMap<VillaUpdateDTO, VillaDTO>();
    options.CreateMap<User, UserDTO>();
    options.CreateMap<VillaAmenitiesUpdateDTO, VillaAmenities>();
    options.CreateMap<VillaAmenitiesUpdateDTO, VillaAmenities>();
    options.CreateMap<VillaAmenities, VillaAmenitiesDTO>().ForMember(dest => dest.VillaName, opt => opt.MapFrom(src => src.Villa!=null? src.Villa.Name:null));
    options.CreateMap<VillaAmenitiesDTO, VillaAmenities>();

});

var app = builder.Build();

await SeedDataAsync(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

//apply pending migration to database
static async Task SeedDataAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();

    var context = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();

    await context.Database.MigrateAsync();
}