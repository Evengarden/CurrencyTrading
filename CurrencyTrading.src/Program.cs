using CurrencyTrading.Data;
using CurrencyTrading.Helper;
using CurrencyTrading.Interfaces;
using CurrencyTrading.Repository;
using CurrencyTrading.services.Helpers;
using CurrencyTrading.services.Interfaces;
using CurrencyTrading.services.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"));
});

builder.Services.AddTransient<IUserRepository,UserRepository>();
builder.Services.AddTransient<ILotRepository,LotRepository>();
builder.Services.AddTransient<ITradeRepository,TradeRepository>();
builder.Services.AddTransient<IBalanceRepository,BalanceRepository>();

builder.Services.AddTransient<IUserService,UserService>();
builder.Services.AddTransient<IBalanceService,BalanceService>();
builder.Services.AddTransient<ILotService,LotService>();
builder.Services.AddTransient<ITradeService,TradeService>();

builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("JWTSettings"));
var secretKey = builder.Configuration.GetSection("JWTSettings:SecretKey").Value;
var issuer = builder.Configuration.GetSection("JWTSettings:Issuer").Value;
var audience = builder.Configuration.GetSection("JWTSettings:Audience").Value;
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = issuer,
        ValidateAudience = true,
        ValidAudience = audience,
        ValidateLifetime = false,
        IssuerSigningKey = signingKey,
        ValidateIssuerSigningKey = true
    };
});

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
