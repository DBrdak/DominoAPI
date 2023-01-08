using DominoAPI.Entities;
using DominoAPI.Middleware;
using DominoAPI.Models.Query;
using DominoAPI.Models.Query.Fleet;
using DominoAPI.Services;
using Duende.IdentityServer.Validation;
using FluentValidation;
using FluentValidation.AspNetCore;
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
                    .AddJsonFile("appsettings.json")
                    .Build();

                builder.Services.AddControllers();

                builder.Services.AddDbContext<DominoDbContext>();

                builder.Services.AddScoped<Seeder>();

                builder.Services.AddScoped<IButcheryService, ButcheryService>();
                builder.Services.AddScoped<IPriceListService, PriceListService>();
                builder.Services.AddScoped<IShopsService, ShopsService>();
                builder.Services.AddScoped<IFleetService, FleetService>();

                builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
                builder.Services.AddControllers().AddFluentValidation();

                builder.Services.AddScoped<RequestTimeMiddleware>();
                builder.Services.AddScoped<ErrorHandlingMiddleware>();

                builder.Services.AddScoped<IValidator<FuelSuppliesQueryParams>, FuelSuppliesQueryValidator>();
                builder.Services.AddScoped<IValidator<FuelNotesQueryParams>, FuelNotesQueryValidator>();
                builder.Services.AddScoped<IValidator<SalesQueryParams>, SalesQueryValidator>();

                builder.Logging.ClearProviders();
                builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                builder.Host.UseNLog();

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

                void SeedDatabase()
                {
                    using var scope = app.Services.CreateScope();
                    try
                    {
                        var scopedContext = scope.ServiceProvider.GetRequiredService<DominoDbContext>();
                        Seeder.Seed(scopedContext);
                    }
                    catch
                    {
                        throw;
                    }
                }
                SeedDatabase();

                app.UseMiddleware<RequestTimeMiddleware>();
                app.UseMiddleware<ErrorHandlingMiddleware>();

                app.UseHttpsRedirection();

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "DominoAPI");
                });

                app.UseAuthorization();

                app.MapControllers();

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