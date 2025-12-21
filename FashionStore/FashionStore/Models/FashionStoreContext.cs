using System.Data.Entity;

namespace FashionStore.Models
{
    public class FashionStoreContext : DbContext
    {
        public FashionStoreContext() : base("name=FashionStoreContext")
        {
            // Tắt Database Initializer vì bạn đã có Database sẵn
            Database.SetInitializer<FashionStoreContext>(null);
        }

        // --- Các DbSet đã tạo/cập nhật ---
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryGroup> CategoryGroups { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Color> Colors { get; set; }

        // --- Người dùng và Đơn hàng ---
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<CustomerAddress> CustomerAddresses { get; set; }
        public DbSet<Role> Roles { get; set; }

        // --- Đánh giá sản phẩm - MỚI THÊM ---
        public DbSet<ProductReview> ProductReviews { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // 1. Ánh xạ quan hệ Nhiều-Nhiều (Product <-> Category)
            modelBuilder.Entity<Product>()
                .HasRequired(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            // 2. Ánh xạ quan hệ Nhiều-Nhiều (AppUser <-> Role)
            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.Roles)
                .WithMany(r => r.AppUsers)
                .Map(mc =>
                {
                    mc.MapLeftKey("UserId");     // Tên cột FK trong bảng trung gian UserRole
                    mc.MapRightKey("Rid");       // Tên cột FK trong bảng trung gian UserRole
                    mc.ToTable("UserRole");      // Tên bảng trung gian
                });

            // 3. Ánh xạ tên bảng Order (Vì là từ khóa SQL)
            modelBuilder.Entity<Order>().ToTable("Order");

            // 4. Ánh xạ quan hệ ProductReview -> OrderItem
            modelBuilder.Entity<ProductReview>()
                .HasRequired(r => r.OrderItem)
                .WithMany()
                .HasForeignKey(r => r.OrderItemId)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}