// Подключение необходимого пространства имен для работы с Entity Framework
using Microsoft.EntityFrameworkCore;
// Подключение пространства имен для работы с моделями данных
using WebApi.Models;

namespace WebApi
{
    // Класс контекста базы данных, наследующий DbContext
    public class ApplicationDbContext : DbContext
    {
        // DbSet для работы с сущностями "Category" (категории)
        public DbSet<Category> Categories { get; set; }
        // DbSet для работы с сущностями "Candy" (сладости)
        public DbSet<Candy> Candies { get; set; }
        // DbSet для работы с сущностями "Role" (роли)
        public DbSet<Role> Roles { get; set; }
        // DbSet для работы с сущностями "Client" (клиенты)
        public DbSet<Client> Clients { get; set; }
        // DbSet для работы с сущностями "Cart" (корзины)
        public DbSet<Cart> Carts { get; set; }
        // DbSet для работы с сущностями "CartItem" (элементы корзины)
        public DbSet<CartItem> CartItems { get; set; }
        // DbSet для работы с сущностями "Order" (заказы)
        public DbSet<Order> Orders { get; set; }
        // DbSet для работы с сущностями "OrderItem" (элементы заказа)
        public DbSet<OrderItem> OrderItems { get; set; }

        // Конструктор, принимающий параметры для конфигурации DbContext
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Метод для конфигурации модели данных и связей между сущностями
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка связи между сущностью "Candy" и "Category"
            modelBuilder.Entity<Candy>()
                // Указывает, что у каждой сладости есть одна категория
                .HasOne(s => s.Category)
                // Указывает, что у каждой категории может быть множество сладостей
                .WithMany(b => b.Candies)
                // Указывает, что связь будет использована через внешний ключ "CategoryId"
                .HasForeignKey(s => s.CategoryId);

            // Настройка уникального индекса для свойства "Name" в сущности "Role"
            modelBuilder.Entity<Role>()
                .HasIndex(b => b.Name) // Создание индекса для поля "Name"
                .IsUnique(); // Уникальный индекс, гарантирует уникальность значений

            // Настройка уникального индекса для свойства "Name" в сущности "Category"
            modelBuilder.Entity<Category>()
                .HasIndex(b => b.Name) // Создание индекса для поля "Name"
                .IsUnique(); // Уникальный индекс для поля "Name"

            // Настройка уникального индекса для свойства "Username" в сущности "Client"
            modelBuilder.Entity<Client>()
                .HasIndex(c => c.Username) // Создание индекса для поля "Username"
                .IsUnique(); // Уникальный индекс для поля "Username"

            // Настройка уникального индекса для свойства "Email" в сущности "Client"
            modelBuilder.Entity<Client>()
                .HasIndex(c => c.Email) // Создание индекса для поля "Email"
                .IsUnique(); // Уникальный индекс для поля "Email"

            // Настройка уникального индекса для свойства "Phone" в сущности "Client"
            modelBuilder.Entity<Client>()
                .HasIndex(c => c.Phone) // Создание индекса для поля "Phone"
                .IsUnique(); // Уникальный индекс для поля "Phone"

            // Настройка связи между сущностями "Client" и "Role"
            modelBuilder.Entity<Client>()
                // Указывает, что у клиента есть одна роль
                .HasOne(s => s.Role)
                // Указывает, что роль может быть присвоена многим клиентам
                .WithMany(b => b.Clients)
                // Указывает внешний ключ для связи "RoleId"
                .HasForeignKey(s => s.RoleId);

            // Настройка связи между сущностями "Cart" и "Client"
            modelBuilder.Entity<Cart>()
                // Указывает, что у корзины есть один клиент
                .HasOne(s => s.Client)
                // Указывает, что у клиента может быть много корзин
                .WithMany(s => s.Carts)
                // Указывает внешний ключ для связи "ClientId"
                .HasForeignKey(s => s.ClientId);

            // Настройка связи между сущностями "CartItem" и "Cart"
            modelBuilder.Entity<CartItem>()
                // Указывает, что элемент корзины принадлежит одной корзине
                .HasOne(s => s.Cart)
                // Указывает, что корзина может содержать много элементов
                .WithMany(s => s.CartItems)
                // Указывает внешний ключ для связи "CartId"
                .HasForeignKey(s => s.CartId);

            // Настройка связи между сущностями "CartItem" и "Candy"
            modelBuilder.Entity<CartItem>()
                // Указывает, что элемент корзины связан с одной сладостью
                .HasOne(s => s.Candy)
                // Указывает, что сладость может быть в многих элементах корзины
                .WithMany(s => s.CartItems)
                // Указывает внешний ключ для связи "CandyId"
                .HasForeignKey(s => s.CandyId);

            // Настройка связи между сущностями "Order" и "Client"
            modelBuilder.Entity<Order>()
                 // Указывает, что заказ принадлежит одному клиенту
                 .HasOne(s => s.Client)
                 // Указывает, что у клиента может быть много заказов
                 .WithMany(b => b.Orders)
                 // Указывает внешний ключ для связи "ClientId"
                 .HasForeignKey(s => s.ClientId);

            // Настройка связи между сущностями "OrderItem" и "Candy"
            modelBuilder.Entity<OrderItem>()
                // Указывает, что элемент заказа связан с одной сладостью
                .HasOne(s => s.Candy)
                // Указывает, что сладость может быть в многих элементах заказов
                .WithMany(s => s.OrderItems)
                // Указывает внешний ключ для связи "CandyId"
                .HasForeignKey(s => s.CandyId);

            // Настройка связи между сущностями "OrderItem" и "Order"
            modelBuilder.Entity<OrderItem>()
                // Указывает, что элемент заказа связан с одним заказом
                .HasOne(s => s.Order)
                // Указывает, что заказ может содержать много элементов
                .WithMany(s => s.OrderItems)
                // Указывает внешний ключ для связи "OrderId"
                .HasForeignKey(s => s.OrderId);
        }
    }
}
