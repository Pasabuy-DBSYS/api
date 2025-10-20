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
using PasabuyAPI.Enums;
using PasabuyAPI.Exceptions;
using Microsoft.AspNetCore.Identity;
using PasabuyAPI.Models;
using Amazon.S3;
using Amazon;


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
builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();

// Dependency Injections [Services]
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IDeliveryDetailsService, DeliveryDetailsService>();
builder.Services.AddScoped<IPaymentsService, PaymentsService>();
builder.Services.AddScoped<IVerificationInfoService, VerificationInfoService>();
builder.Services.AddScoped<IChatMessagesService, ChatMessagesService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IAwsS3Service, AwsS3Service>();


// PasswordHasher DI
builder.Services.AddScoped<IPasswordHasher<Users>, PasswordHasher<Users>>();

// Mappers
MapsterConfig.RegisterMappings();
builder.Services.AddMapster(); // if using Mapster.DependencyInjection

// JWT
builder.Services.AddSingleton<TokenProvider>();

// Authentication AND Autherization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("VerifiedOnly", policy =>
        policy.RequireClaim("Verification Status", VerificationInfoStatus.ACCEPTED.ToString()));

    options.AddPolicy("CourierOnly", policy =>
        policy.RequireRole(Roles.COURIER.ToString())
            .RequireClaim("Verification Status", VerificationInfoStatus.ACCEPTED.ToString()));

    options.AddPolicy("CustomerOnly", policy =>
        policy.RequireRole(Roles.CUSTOMER.ToString())
            .RequireClaim("Verification Status", VerificationInfoStatus.ACCEPTED.ToString()));
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"] ?? throw new NotFoundException("Jwt Secret is not found or empty"))),
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ClockSkew = TimeSpan.Zero
        };

        o.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chatsHub"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });

// Register Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenWithAuth();

// SignalR Service
builder.Services.AddSignalR();

// AWS S3
var awsOptions = builder.Configuration.GetSection("AWS");

builder.Services.AddSingleton<IAmazonS3>(sp =>
{
    return new AmazonS3Client(
        awsOptions["AccessKey"],
        awsOptions["SecretKey"],
        RegionEndpoint.GetBySystemName(awsOptions["Region"])
    );
});

// ✅ Add CORS (for frontend connection)
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowFrontend",
//         policy => policy
//             .WithOrigins("http://localhost:5173") // React frontend URL
//             .AllowAnyHeader()
//             .AllowAnyMethod()
//             .AllowCredentials());
// });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy
            .WithOrigins("http://localhost:5173")
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
app.UseCors("AllowAll");
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();
app.MapHub<ChatHub>("/chatsHub");  // Chat hub endpoint
app.MapHub<OrdersHub>("/ordersHub"); // Orders hub endpoint
app.Run();
