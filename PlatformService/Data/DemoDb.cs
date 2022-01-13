using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.Models;

namespace PlatformService.Data;

public static class DemoDb
{
    public static void PrepPopulation(IApplicationBuilder app, bool isProduction)
    {
        using IServiceScope serviceScope = app.ApplicationServices.CreateScope();
        SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProduction);
    }

    private static void SeedData(AppDbContext? context, bool isProduction)
    {
        if (isProduction)
        {
            Console.WriteLine("--> Applying migrations");
            try
            {
                context?.Database.Migrate();   
            }
            catch (Exception e)
            {
                Console.WriteLine("--> Error occured when applying migrations. Error message:");
                Console.WriteLine(e);
            }
        }
        
        if (context is null || context.Platforms.Any()) return;
        
        context.Platforms.AddRange(
            new Platform { Name = "Dot Net", Publisher = "Microsoft", Cost = "200" },
            new Platform { Name = "SQL Server", Publisher = "Microsoft", Cost = "Free" },
            new Platform { Name = "Kubernetes", Publisher = "Cloud Native Computing", Cost = "Free" }
        );

        context.SaveChanges();
    }
}