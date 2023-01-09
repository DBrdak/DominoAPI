using System.Text;
using DominoAPI.Authorization;
using DominoAPI.Entities;
using DominoAPI.Entities.Accounts;
using DominoAPI.Middleware;
using DominoAPI.Models;
using DominoAPI.Models.AccountModels;
using DominoAPI.Models.Query;
using DominoAPI.Models.Query.Fleet;
using DominoAPI.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Web;

namespace DominoAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
            logger.Debug("init main");

            try
            {
                var builder = WebApplication.CreateBuilder(args);

                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.Development.json")
                    .Build();

                builder.Services.AddControllers().AddFluentValidation();

                builder.Services.AddDbContext<DominoDbContext>
                    (options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

                builder.Services.AddScoped<Seeder>();

                builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

                //Authentication
                {
                    var authenticationSettings = new AuthenticationSettings();
                    configuration.GetSection("Authentication").Bind(authenticationSettings);

                    builder.Services.AddSingleton(authenticationSettings);

                    builder.Services.AddAuthentication(option =>
                    {
                        option.DefaultAuthenticateScheme = "Bearer";
                        option.DefaultScheme = "Bearer";
                        option.DefaultChallengeScheme = "Bearer";
                    }).AddJwtBearer(cfg =>
                    {
                        cfg.RequireHttpsMetadata = false;
                        cfg.SaveToken = true;
                        cfg.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidIssuer = authenticationSettings.JwtIssuer,
                            ValidAudience = authenticationSettings.JwtIssuer,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey))
                        };
                    });

                    builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
                }

                //Authorization
                {
                    builder.Services.AddScoped<IAuthorizationHandler, ResourceOperationRequirementHandler>();
                }

                //Services
                {
                    builder.Services.AddScoped<IButcheryService, ButcheryService>();
                    builder.Services.AddScoped<IPriceListService, PriceListService>();
                    builder.Services.AddScoped<IShopsService, ShopsService>();
                    builder.Services.AddScoped<IFleetService, FleetService>();
                    builder.Services.AddScoped<IAccountService, AccountService>();
                    builder.Services.AddScoped<IUserContextService, UserContextService>();
                }

                //Middlewares
                {
                    builder.Services.AddScoped<RequestTimeMiddleware>();
                    builder.Services.AddScoped<ErrorHandlingMiddleware>();
                }

                //Validators
                {
                    builder.Services.AddScoped<IValidator<FuelSuppliesQueryParams>, FuelSuppliesQueryValidator>();
                    builder.Services.AddScoped<IValidator<FuelNotesQueryParams>, FuelNotesQueryValidator>();
                    builder.Services.AddScoped<IValidator<SalesQueryParams>, SalesQueryValidator>();
                    builder.Services.AddScoped<IValidator<UserQueryParams>, UserQueryValidator>();
                    builder.Services.AddScoped<IValidator<RegisterUserDto>, AccountValidator>();
                    builder.Services.AddScoped<IValidator<UpdateUserDto>, UserUpdateValidator>();
                }

                //Logger
                {
                    builder.Logging.ClearProviders();
                    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                    builder.Host.UseNLog();
                }

                builder.Services.AddHttpContextAccessor();

                builder.Services.AddSwaggerGen();

                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("FrontEndClient", builder =>
                        builder.AllowAnyMethod()
                            .AllowAnyHeader()
                            .WithOrigins(configuration["AllowedOrigins"])
                    );
                });

                var app = builder.Build();

                app.UseStaticFiles();
                app.UseCors("FrontEndClient");

                async Task SeedDatabase()
                {
                    using var scope = app.Services.CreateScope();
                    try
                    {
                        var scopedContext = scope.ServiceProvider.GetRequiredService<DominoDbContext>();
                        await Seeder.Seed(scopedContext);
                    }
                    catch
                    {
                        throw;
                    }
                }
                await SeedDatabase();

                app.UseMiddleware<RequestTimeMiddleware>();
                app.UseMiddleware<ErrorHandlingMiddleware>();

                app.UseAuthentication();

                app.UseHttpsRedirection();

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "DominoAPI");
                });

                app.MapControllers();

                app.UseAuthorization();

                await app.RunAsync();
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }
    }
}