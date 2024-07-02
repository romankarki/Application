using Domain.Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence
{
    public static class Populate
    {
        public static async Task<IApplicationBuilder> PrepareDatabase(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                await PopulateData(serviceScope.ServiceProvider.GetService<AppDbContext>());
            }
            return app;

        }

        public static async Task PopulateData(AppDbContext context)
        {
            System.Console.WriteLine("Applying migrations .......");
            await context.Database.MigrateAsync();
            System.Console.WriteLine("End of migrations ");
            if (!context.Tests.Any())
            {
                System.Console.WriteLine("Starting to seed data ");
                context.Tests.AddRange(
                    new Test { Name = "ABC" },
                    new Test { Name = "DEF" }
                );
                context.SaveChanges();
            }
            if (!context.Officers.Any())
            {
                System.Console.WriteLine("Starting to seed data ");
                context.Officers.AddRange(
                    new Officer
                    {
                        IdentificationNumber = "ID11",
                        Password = "$2a$11$pJIXz997AlT0kb1T3YVbQu7rK/pe2XITfIioh8h01qpFPwpilK7Iu",
                        ContactEmail = "testemail@user.com",
                        ContactNumber = "9800900090",
                        CreatedBy = 0,
                        UpdatedBy = 0,
                        CreatedDate = DateTime.UtcNow,
                        UpdatedDate = DateTime.UtcNow,
                        Name = "Test User"
                    });
                context.SaveChanges();
            }

            System.Console.WriteLine("End of seeding data");
        }
    }
}
