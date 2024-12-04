using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Candy> Candies { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base( options ) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Candy>()
                .HasOne(s => s.Category)
                .WithMany(b => b.Candies)
                .HasForeignKey(s => s.CategoryId);

            modelBuilder.Entity<Role>()
                .HasIndex(b => b.Name)
                .IsUnique();

            modelBuilder.Entity<Category>()
                .HasIndex(b => b.Name)
                .IsUnique();

            modelBuilder.Entity<Client>()
                .HasIndex(c => c.Username)
                .IsUnique();

            modelBuilder.Entity<Client>()
                .HasIndex(c => c.Email)
                .IsUnique();

            modelBuilder.Entity<Client>()
                .HasIndex(c => c.Phone)
                .IsUnique();

            modelBuilder.Entity<Client>()
                .HasOne(s => s.Role)
                .WithMany(b => b.Clients)
                .HasForeignKey(s => s.RoleId);

            modelBuilder.Entity<Cart>()
                .HasOne(s => s.Client)
                .WithMany(s => s.Carts)
                .HasForeignKey(s => s.ClientId);

            modelBuilder.Entity<CartItem>()
                .HasOne(s => s.Cart)
                .WithMany(s => s.CartItems)
                .HasForeignKey(s => s.CartId);

            modelBuilder.Entity<CartItem>()
                .HasOne(s => s.Candy)
                .WithMany(s => s.CartItems)
                .HasForeignKey(s => s.CandyId);

            modelBuilder.Entity<Order>()
                 .HasOne(s => s.Client)
                 .WithMany(b => b.Orders)
                 .HasForeignKey(s => s.ClientId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(s => s.Candy)
                .WithMany(s => s.OrderItems)
                .HasForeignKey(s => s.CandyId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(s => s.Order)
                .WithMany(s => s.OrderItems)
                .HasForeignKey(s => s.OrderId);
        }
    }
}
