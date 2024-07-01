using Domain.Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence
{
    public static class Populate
    {
        public static async Task<IApplicationBuilder>  PrepareDatabase(this IApplicationBuilder app)
        {
            using(var serviceScope = app.ApplicationServices.CreateScope())
            {
                await  PopulateData(serviceScope.ServiceProvider.GetService<AppDbContext>()); 
            }
            return app;

        }

        public static async Task PopulateData(AppDbContext context) 
        {
            System.Console.WriteLine("Applying migrations .......");
            await context.Database.MigrateAsync();
            System.Console.WriteLine("End of migrations ");
            if(!context.Tests.Any())
            {
                System.Console.WriteLine("Starting to seed data ");
                context.Tests.AddRange(
                    new Test {Name = "ABC" },
                    new Test { Name ="DEF"}
                );
                context.SaveChanges();
            }
            System.Console.WriteLine("End of seeding data");
        }
    }
}
