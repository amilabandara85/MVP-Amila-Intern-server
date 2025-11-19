using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using AmilaOnboarding.Server.Models;
using System.Text.Json.Serialization;


// Define the name for your CORS policy outside of the main configuration block
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins"; // <-- NEW LINE

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// 1. ADD CORS SERVICES
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            // IMPORTANT: Replace http://localhost:5173 with your actual React/Vite development server URL if it's different.
            policy.WithOrigins("http://localhost:5173",
                "https://amila-mvponboarding-web.azurewebsites.net")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
}); // <-- NEW BLOCK END

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    //options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    //options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AmilaOnboardingContext>(Option =>
    Option.UseSqlServer(builder.Configuration.GetConnectionString("AzureSQLConnection")));

builder.Services.AddDbContext<AmilaOnboardingContext>(Option =>
      Option.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Initial Catalog=Amila-Onboarding;Integrated Security=True;TrustServerCertificate=Yes"));
     


var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 2. USE CORS MIDDLEWARE (MUST BE BEFORE app.UseAuthorization())
app.UseCors(MyAllowSpecificOrigins); // <-- NEW LINE

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
