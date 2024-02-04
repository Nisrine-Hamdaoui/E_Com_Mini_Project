using Humanizer.Localisation;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using E_Com_Project.Models;

namespace E_Com_Project.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<E_Com_Project.Models.Product> Products { get; set; } = default!;
        public DbSet<E_Com_Project.Models.Category> Categories { get; set; } = default!;

        public DbSet<E_Com_Project.Models.Cart> Carts { get; set; } = default!;
        public DbSet<E_Com_Project.Models.CartLine> CartLines { get; set; } = default!;
        public DbSet<E_Com_Project.Models.Order> Orders { get; set; } = default!;
        public DbSet<E_Com_Project.Models.OrderDetails> OrderDetails { get; set; } = default!;

        public DbSet<E_Com_Project.Models.OrderStatus> OrderStatuses { get; set; } = default!;

    }
}
