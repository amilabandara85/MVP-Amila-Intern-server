using Microsoft.EntityFrameworkCore;
using AmilaOnboarding.Server.Models;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// 1. ADD CORS SERVICES
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins(
                "https://localhost:55677", // Local Server HTTPS
                "https://localhost:55676", // Local Server HTTPS
                "http://localhost:5173",   // Local Client (Vite) HTTP
                "https://amila-mvponboarding-web.azurewebsites.net") // Azure Deployed Client/Server
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Use the connection string from appsettings.json

builder.Services.AddDbContext<AmilaOnboardingContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("AzureSQLConnection"),
        sqlOptions =>
        {
            // Enable retry logic (better known as connection resiliency)
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 10,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
        }));

//builder.Services.AddDbContext<AmilaOnboardingContext>(Option =>
//    Option.UseSqlServer(builder.Configuration.GetConnectionString("AzureSQLConnection"))); // CORRECTLY READS FROM appsettings.json

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 2. USE CORS MIDDLEWARE
app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();