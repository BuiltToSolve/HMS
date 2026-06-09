using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Configure Ocelot JSON configuration based on environment
builder.Configuration.AddJsonFile(
    builder.Environment.IsDevelopment() ? "ocelot.dev.json" : "ocelot.json", 
    optional: false, 
    reloadOnChange: true);

// Add Ocelot services to the container
builder.Services.AddOcelot();

// Add CORS to allow the frontend to easily query endpoints via the Gateway
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy
            .AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

app.UseCors("AllowAll");

app.MapGet("/", () => "API Gateway is running.");

// Run Ocelot Middleware
await app.UseOcelot();

app.Run();
