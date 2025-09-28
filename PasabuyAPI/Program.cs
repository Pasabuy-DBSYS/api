using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using PasabuyAPI.Data;
using PasabuyAPI.Repositories.Interfaces;
using PasabuyAPI.Repositories.Implementations;
using PasabuyAPI.Services.Interfaces;
using PasabuyAPI.Services.Implementations;
using System.Text.Json.Serialization;
using PasabuyAPI.Configurations.Mapping;
using Mapster;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers().AddJsonOptions(x => 
        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
    
builder.Services.AddDbContext<PasabuyDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Dependency Injections [Repositories]
builder.Services.AddScoped<IUserRespository, UserRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Dependency Injections [Services]
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOrderService, OrderService>();


// Mappers
MapsterConfig.RegisterMappings();
builder.Services.AddMapster(); // if using Mapster.DependencyInjection

// Register Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My Web API",
        Version = "v1",
        Description = "API documentation for My Web API project"
    });
});

builder.Services.AddMvc(options =>
{
   options.SuppressAsyncSuffixInActionNames = false;
});

var app = builder.Build();

// Enable Swagger middleware in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Web API v1");
        c.RoutePrefix = string.Empty; // Swagger UI at app root
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
