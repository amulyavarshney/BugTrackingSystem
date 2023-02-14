namespace BugTrackingSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // dependency injection configuration
            builder.Services.AddDbContext<BugTrackingContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Development"));
            });
            builder.Services.AddControllers();
            builder.Services.AddScoped<IProductService, ProductService>();

            var app = builder.Build();

            // middleware configuration
            app.MapControllers();

            app.Run();
        }
    }
}