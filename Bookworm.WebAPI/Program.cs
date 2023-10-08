using Microsoft.AspNetCore.HttpLogging;
using RestSharp;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddHttpClient();
builder.Services.AddSingleton<IRestClient>(sp => new RestClient("https://openlibrary.org"));
builder.Services.AddControllers();
builder.Services.AddCors();
builder.Services.AddDistributedMemoryCache();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Configure NSwag
builder.Services.AddOpenApiDocument(config =>
{
    config.Title = "Bookworm API";
    config.Version = "v1";
});

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
});

var app = builder.Build();

app.UseHttpLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Use NSwag middleware to serve the OpenAPI spec and Swagger UI
    app.UseOpenApi();  // Serves the OpenAPI specification
    app.UseSwaggerUi3(); // Serves the Swagger UI
}

app.UseCors(policy =>
    policy.AllowAnyOrigin()
          .AllowAnyHeader()
          .AllowAnyMethod());

app.UseAuthorization();

app.MapControllers();

app.Run();

