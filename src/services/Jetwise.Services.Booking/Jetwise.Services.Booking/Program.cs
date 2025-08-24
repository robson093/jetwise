using Jetwise.Services.Booking.Controllers;
using Jetwise.Services.Booking.Services;
using Jetwise.Services.Booking.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://dev-onuukffx52o7o3w3.us.auth0.com/"; // domena Auth0, np. your-tenant.auth0.com
        options.Audience = "https://localhost:5184/"; // audience, którego oczekujesz (API identifier w Auth0)

        // Jeśli chcesz, możesz włączyć dodatkowe opcje, np. sprawdzanie HTTPS itp.
        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = "sub",
            ValidateAudience = false,
            ValidateIssuer = false
            // ValidateAudience = true,
            // ValidAudience = "https://localhost:5000/",
            // ValidateIssuer = true,
            // ValidIssuer = "dev-onuukffx52o7o3w3.us.auth0.com/"
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("JWT auth failed: " + context.Exception.Message);
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("JWT token validated.");
                return Task.CompletedTask;
            }
        };
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddUserSecrets<Program>() // <-- To dodaje user-secrets
    .AddEnvironmentVariables();

builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
{
    var user = Environment.GetEnvironmentVariable("MONGO_USER");
    var pass = Environment.GetEnvironmentVariable("MONGO_PASS");
    var host = Environment.GetEnvironmentVariable("MONGO_HOST");
    var connectionString = $"mongodb://{user}:{pass}@{host}:27017/?authSource=admin";
    builder.Configuration["MongoDbSettings:ConnectionString"] = connectionString;
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

builder.Services.AddScoped<ISampleDocumentsService, SampleDocumentsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
