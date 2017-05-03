using Microsoft.EntityFrameworkCore;
using SimpleCart.Models;
using System;

namespace SimpleCart.Context
{
    public class SimpleCartContext : DbContext
    {
        public SimpleCartContext(DbContextOptions<SimpleCartContext> options) : base(options) { }

        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasKey(c => c.ID);
            modelBuilder.Entity<ShoppingCart>().HasKey(c => c.ID);
            modelBuilder.Entity<ShoppingCart>().HasOne<Product>(c => c.Product);
            base.OnModelCreating(modelBuilder);
        }

        public new bool SaveChanges()
        {
            //TODO: Log Exceptions
            try
            {
                base.SaveChanges();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

            catch (DbUpdateException)
            {
                return false;
            }
            catch(NotSupportedException)
            {
                return false;
            }
            catch(ObjectDisposedException)
            {
                return false;
            }
            catch (InvalidOperationException)
            {
                return false;
            }

        }
    }
}
