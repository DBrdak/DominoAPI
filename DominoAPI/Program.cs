using DominoAPI.Entities;
using DominoAPI.Middleware;
using DominoAPI.Services;
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

                builder.Services.AddControllers();

                builder.Services.AddDbContext<DominoDbContext>();

                builder.Services.AddScoped<Seeder>();

                builder.Services.AddScoped<IButcheryService, ButcheryService>();
                builder.Services.AddScoped<IPriceListService, PriceListService>();
                builder.Services.AddScoped<IShopsService, ShopsService>();
                builder.Services.AddScoped<IFleetService, FleetService>();

                builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

                builder.Services.AddScoped<RequestTimeMiddleware>();
                builder.Services.AddScoped<ErrorHandlingMiddleware>();

                builder.Logging.ClearProviders();
                builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                builder.Host.UseNLog();

                builder.Services.AddSwaggerGen();

                var app = builder.Build();

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