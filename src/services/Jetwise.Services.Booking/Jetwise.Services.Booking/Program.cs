using Jetwise.Services.Booking.Controllers;
using Jetwise.Services.Booking.Services;
using Jetwise.Services.Booking.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
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
