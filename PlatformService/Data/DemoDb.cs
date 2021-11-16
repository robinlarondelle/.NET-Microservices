using PlatformService.Data;
using PlatformService.Models;

namespace PlatformService.Datal;

public static class DemoDb
{
    public static void PrepPopulation(IApplicationBuilder app)
    {
        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
        }
    }

    private static void SeedData(AppDbContext? context)
    {
        if (context is not null)
        {
            if (!context.Platforms.Any())
            {
                context.Platforms.AddRange(
                    new Platform { Id = 1, Name = "Dot Net", Publisher = "Microsoft", Cost = "200" },
                    new Platform { Id = 2, Name = "SQL Server", Publisher = "Microsoft", Cost = "Free" },
                    new Platform { Id = 3, Name = "Kubernetes", Publisher = "Cloud Native Computing", Cost = "Free" }
                );

                context.SaveChanges();
            }
        }
    }
}