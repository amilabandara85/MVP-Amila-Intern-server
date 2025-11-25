using Microsoft.EntityFrameworkCore;
using AmilaOnboarding.Server.Models;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);


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


builder.Services.AddDbContext<AmilaOnboardingContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//sqlOptions =>
//                    {

//                    sqlOptions.EnableRetryOnFailure(
//                    maxRetryCount: 10,
//                    maxRetryDelay: TimeSpan.FromSeconds(30),
//                    errorNumbersToAdd: null);

//                    }));



var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();