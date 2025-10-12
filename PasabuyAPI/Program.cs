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
using PasabuyAPI.Hubs;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PasabuyAPI.Configurations.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using PasabuyAPI.Configurations.Extensions;

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
builder.Services.AddScoped<IDeliveryDetailsRepository, DeliveryDetailsRepository>();
builder.Services.AddScoped<IPaymentsRepository, PaymentsRepository>();
builder.Services.AddScoped<IVerificationInfoRepository, VerificationInfoRepository>();
builder.Services.AddScoped<IChatMessagesRepository, ChatMessagesRepository>();
builder.Services.AddScoped<IChatRoomRepository, ChatRoomRepository>();

// Dependency Injections [Services]
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IDeliveryDetailsService, DeliveryDetailsService>();
builder.Services.AddScoped<IPaymentsService, PaymentsService>();
builder.Services.AddScoped<IVerificationInfoService, VerificationInfoService>();
builder.Services.AddScoped<IChatMessagesService, ChatMessagesService>();


// Mappers
MapsterConfig.RegisterMappings();
builder.Services.AddMapster(); // if using Mapster.DependencyInjection

// JWT
builder.Services.AddSingleton<TokenProvider>();

// Authentication AND Autherization
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"])),
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ClockSkew = TimeSpan.Zero
        };
    });

// Register Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenWithAuth();

// SignalR Service
builder.Services.AddSignalR();

// ✅ Add CORS (for frontend connection)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy
            .WithOrigins("http://localhost:5173") // React frontend URL
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

builder.Services.AddMvc(options =>
{
   options.SuppressAsyncSuffixInActionNames = false;
});

var app = builder.Build();


// Websockets

app.MapHub<OrdersHub>("/ordersHub"); // hub endpoint
app.MapHub<ChatHub>("/chatsHub");


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

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
