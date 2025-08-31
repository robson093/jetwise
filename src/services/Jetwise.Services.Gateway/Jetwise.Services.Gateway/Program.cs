using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

//w ocelot.json pod 
// "UpstreamHttpMethod": [
//"GET"
//            ]
//
//
//,
//            "AuthenticationOptions": {
//    "AuthenticationProviderKey": "Bearer",
//                "AllowedScopes": []
//            }

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = "https://dev-onuukffx52o7o3w3.us.auth0.com/";
    options.Audience = "https://jetwise-gateway/";
});

builder.Services.AddCors(options =>
options.AddPolicy("JetwiseClient", p =>
 p//.WithOrigins("https://127.0.0.1:7289") // SPA origin
 .AllowAnyOrigin()
     .AllowAnyHeader()
     .AllowAnyMethod()
     //.AllowCredentials()
     ));

// Add services to the container.

builder.Services.AddControllers(); 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
 
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
    //.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables(); 
builder.Services.AddOcelot();

var app = builder.Build();

app.UseCors("JetwiseClient");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) 
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//await app.UseOcelot();

 

app.Run();
