namespace Movies.API
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Movies.API.Contexts;

    public class Program
    {
        public static void Main(string[] args)
        {
            IWebHost host = CreateWebHostBuilder(args).Build();

            // For demo purposes: clear the database
            // and refill it with dummy data
            using (IServiceScope scope = host.Services.CreateScope())
            {
                try
                {
                    MoviesContext context = scope.ServiceProvider.GetService<MoviesContext>();
                    // delete the DB if it exists
                    context.Database.EnsureDeleted();
                    // migrate the DB - this will also seed the DB with dummy data
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    ILogger<Program> logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }

            // run the web app
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
