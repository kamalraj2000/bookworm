using RestSharp;

var builder = WebApplication.CreateBuilder(args);

// Read cache provider from configuration
var cacheProvider = builder.Configuration.GetValue<string>("Cache:Provider");

switch (cacheProvider)
{
    
    case "SQL":
        // Add SQL distributed cache. Ensure you have the necessary package and connection string.
        builder.Services.AddDistributedSqlServerCache(options =>
        {
            options.ConnectionString = builder.Configuration.GetConnectionString("CacheDbConnection");
            options.SchemaName = "dbo";
            options.TableName = "BookCache";
        });
        break;
    case "Redis":
        // Add Redis cache. Ensure you have the necessary package and connection string.
        builder.Services.AddDistributedRedisCache(options =>
        {
            options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
        });
        break;
    case "Memory":
    default:
        builder.Services.AddDistributedMemoryCache();
        break;
}

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

var app = builder.Build();

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

