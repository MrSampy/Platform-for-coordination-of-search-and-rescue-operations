using AuthService.API.Core.Interfaces;
using AuthService.API.Core.Models;
using AuthService.API.Core.Services;
using AuthService.API.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddEnvironmentVariables()
    .AddUserSecrets(Assembly.GetExecutingAssembly(), true);
// Add services to the container.

var configuration = builder.Configuration.GetSection("AuthServer").Get<AuthServerSetting>();

var useInMemory = builder.Configuration.GetValue<bool>("UseInMemoryDatabase");

if (useInMemory)
{
    builder.Services.AddDbContext<AuthDbContext>(o => o.EnableSensitiveDataLogging()
        .UseInMemoryDatabase("Test_Database"));
}
else
{
    string connection = builder.Configuration.GetConnectionString("PostgreSQLConnection")!;
    builder.Services.AddDbContext<AuthDbContext>(o => o.UseNpgsql(connection, sqlServerOptions =>
        sqlServerOptions.EnableRetryOnFailure()));
}

builder.Services.AddScoped(typeof(IAuthenticateSevice), typeof(AuthenticateSevice));
builder.Services.AddScoped(typeof(IUserSevice), typeof(UserSevice));
builder.Services.AddScoped(typeof(IRoleSevice), typeof(RoleSevice));

// For Identity
builder.Services
    .AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

// Adding Authentication
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["ValidAudience"],
            ValidIssuer = builder.Configuration["ValidIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Secret"] ?? string.Empty))
        };
    });

builder.Services.AddCors();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseStaticFiles();
app.UseCors(
    b => b
        .WithOrigins((builder.Configuration["OriginUrls"] ?? string.Empty).Split(','))
        .SetIsOriginAllowedToAllowWildcardSubdomains()
        .AllowAnyMethod()
        .AllowAnyHeader()
);


//app.UseHttpsRedirection();

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
