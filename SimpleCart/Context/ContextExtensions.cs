using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Linq;

namespace SimpleCart.Context
{
    public static class ContextExtensions
    {
        public static bool AllMigrationsApplied(this DbContext context)
        {
            var applied = context.GetService<IHistoryRepository>()
                .GetAppliedMigrations()
                .Select(m => m.MigrationId);

            var total = context.GetService<IMigrationsAssembly>()
                .Migrations
                .Select(m => m.Key);

            return !total.Except(applied).Any();
        }

        public static void AddSampleData(this SimpleCartContext context)
        {

            if (context.AllMigrationsApplied())
            {
                if (!context.Products.Any())
                {
                    context.Products.AddRange(
                        new Models.Product() { Code = "P1", Description = "Prodcut 1", Price = 1.23m },
                        new Models.Product() { Code = "P2", Description = "Prodcut 2", Price = 2.63m },
                        new Models.Product() { Code = "P3", Description = "Prodcut 3", Price = 10.33m },
                        new Models.Product() { Code = "P4", Description = "Prodcut 4", Price = 4.53m },
                        new Models.Product() { Code = "P5", Description = "Prodcut 5", Price = 1.67m },
                        new Models.Product() { Code = "P6", Description = "Prodcut 6", Price = 1.36m },
                        new Models.Product() { Code = "P7", Description = "Prodcut 7", Price = 5.32m },
                        new Models.Product() { Code = "P8", Description = "Prodcut 8", Price = 6.54m },
                        new Models.Product() { Code = "P9", Description = "Prodcut 9", Price = 7.21m },
                        new Models.Product() { Code = "P10", Description = "Prodcut 10", Price = 4.86m }
                    );

                    context.SaveChanges();
                }
            }
        }
    }
}
