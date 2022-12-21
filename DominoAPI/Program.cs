using DominoAPI.Entities;

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

            var app = builder.Build();

            //app.UseHttpsRedirection();

            void SeedDatabase()
            {
                using (var scope = app.Services.CreateScope())
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

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}