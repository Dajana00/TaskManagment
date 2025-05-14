using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;
using Trello.Model;
using Trello.Repository.IRepository;
using Trello.Repository;
using Trello.Data;
using Trello.Service;
using Trello.Service.Iservice;
using Microsoft.EntityFrameworkCore;
using Trello.Service.IService;
using Trello.Helpers;
using Serilog;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Globalization;
using Trello.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Trello.Mapper;
using Trello.WebSocket;
using Trello.Service.UpdateUserCommands;
using Trello.Service.UpdateUserCommandPattern;
using Trello.ExeptionHandlingMiddleware;

var builder = WebApplication.CreateSlimBuilder(args);


// 🔹 Konfiguriši Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console()
    .WriteTo.File("logs/app.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(Log.Logger);


// 🔹 Konfiguriši bazu podataka
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));



builder.Services.ConfigureApplicationServices(builder.Configuration);





builder.Services.AddEndpointsApiExplorer();
// Dodavanje CORS servisa
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // Ovdje stavljate URL svog React frontend-a
              .AllowAnyHeader()
              .AllowAnyMethod()
               .AllowCredentials();
    });
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

builder.WebHost.UseUrls("http://localhost:5196");


// Dodajemo konfiguraciju za JSON serializer context globalno
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); // Ako koristiš Enum vrednosti
    });

builder.Services.AddEndpointsApiExplorer();


//-----------------------------AUTENTIFIKACIJA---------------------------------------------------------------------
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})


.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"])),
        ClockSkew = TimeSpan.Zero
    };
});


//----------------------------------------------------------------------------------------------------------------------

builder.Services.AddAutoMapper(typeof(MapperProfile));
builder.Services.AddSignalR();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdateUserFieldCommandHandler).Assembly));



var app = builder.Build();
app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();
app.UseAuthentication(); 
app.UseAuthorization();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();
app.UseCors("AllowSpecificOrigin");  // Ovo omogućava CORS samo za specifične izvore

app.MapHub<CardHub>("/cardHub");
app.Run();

[JsonSerializable(typeof(User))]
[JsonSerializable(typeof(RegisterRequestDto))]
[JsonSerializable(typeof(IEnumerable<User>))]
[JsonSerializable(typeof(IEnumerable<RegisterRequestDto>))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
   


    
}
