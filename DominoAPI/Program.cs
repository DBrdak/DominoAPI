using DominoAPI.Entities;
using DominoAPI.Services;

namespace DominoAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddDbContext<DominoDbContext>();

            builder.Services.AddScoped<Seeder>();

            builder.Services.AddScoped<IPriceListService, PriceListService>();
            builder.Services.AddScoped<IButcheryService, ButcheryService>();
            builder.Services.AddScoped<IShopsService, ShopsService>();
            builder.Services.AddScoped<IFleetService, FleetService>();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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

            //app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}