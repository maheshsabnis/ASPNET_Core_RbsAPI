namespace RbsAPI.Models
{
    public class OrdersDbContext : DbContext
    {
        public DbSet<Orders> Orders { get; set; }
        public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
