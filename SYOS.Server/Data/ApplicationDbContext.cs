using Microsoft.EntityFrameworkCore;
using SYOS.Shared.DTO;

namespace SYOS.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ItemDTO> Items { get; set; }
        public DbSet<StockDTO> Stocks { get; set; }
        public DbSet<ShelfDTO> Shelves { get; set; }
        public DbSet<BillDTO> Bills { get; set; }
        public DbSet<BillItemDTO> BillItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure entity relationships and constraints here
            modelBuilder.Entity<BillItemDTO>()
                .HasOne<BillDTO>()
                .WithMany(b => b.BillItems)
                .HasForeignKey(bi => bi.BillID);

            modelBuilder.Entity<BillItemDTO>()
                .HasOne<ItemDTO>()
                .WithMany()
                .HasForeignKey(bi => bi.ItemCode);

            // Add more configurations as needed
        }
    }
}