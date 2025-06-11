using System.Text;
using System.Text.Json;
using APBD11.Data;
using APBD11.Helpers;
using APBD11.Interfaces;
using APBD11.Middleware;
using APBD11.Models;
using APBD11.Services;
using Microsoft.EntityFrameworkCore;




var builder = WebApplication.CreateBuilder(args);

var jwtConfigureData = builder.Configuration.GetSection("Jwt");

var rulesJson = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "example_validation_rules.json"));
var validationRules = JsonSerializer.Deserialize<ValidationRuleSet>(rulesJson,
    new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
builder.Services.AddSingleton(validationRules);

builder.Services.Configure<JwtOptions>(jwtConfigureData);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IDeviceService, DeviceService>();


builder.Services.AddControllers();


var app = builder.Build();

app.UseMiddleware<AdditionalPropertiesValidationMiddleware>();


app.UseRouting();


app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new { Message = "An unexpected error occurred." });
    });
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();