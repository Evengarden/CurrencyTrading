using CurrencyTrading.Data;
using Quartz;
using CurrencyTrading.Interfaces;
using CurrencyTrading.Repository;
using CurrencyTrading.services.Helpers;
using CurrencyTrading.services.Interfaces;
using CurrencyTrading.services.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CurrencyTrading.DAL.Mapping;
using CurrencyTrading.DAL.Repository;
using CurrencyTrading.DAL.Interfaces;

namespace CurrencyTrading
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration =  configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<DataContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("PostgresConnection"));
            });

            services.AddAutoMapper(typeof(AppMappingProfiles));

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ILotRepository, LotRepository>();
            services.AddTransient<ITradeRepository, TradeRepository>();
            services.AddTransient<IBalanceRepository, BalanceRepository>();
            services.AddTransient<ICurrencyRepository, CurrencyRepository>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBalanceService, BalanceService>();
            services.AddScoped<ILotService, LotService>();
            services.AddScoped<ITradeService, TradeService>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddStackExchangeRedisCache(options => {
                options.Configuration = "redis:6379,abortConnect=false";
            });

            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionScopedJobFactory();
                var getCurrencyJobKey = new JobKey("GetCurrencyFromCb");
                q.AddJob<CurrencyService>(opts => opts.WithIdentity(getCurrencyJobKey));

                q.AddTrigger(opts => opts
                    .ForJob(getCurrencyJobKey)
                    .WithIdentity("GetCurrencyFromCb-trigger")
                    .WithCronSchedule("0 0 0 * * ?")
                );

                var startAutomatchingJobKey = new JobKey("Automatching");
                q.AddJob<MatchingService>(opts => opts.WithIdentity(startAutomatchingJobKey));

                q.AddTrigger(opts => opts
                    .ForJob(startAutomatchingJobKey)
                    .WithIdentity("Automatching-trigger")
                    .WithCronSchedule("0 */10 * ? * *")
                );
            });
            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

            services.Configure<JWTSettings>(Configuration.GetSection("JWTSettings"));
            var secretKey = Configuration.GetSection("JWTSettings:SecretKey").Value;
            var issuer = Configuration.GetSection("JWTSettings:Issuer").Value;
            var audience = Configuration.GetSection("JWTSettings:Audience").Value;
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            services.AddAuthentication(options =>
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
        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<DataContext>();
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }
            }

            app.Run();
        }
    }
}
