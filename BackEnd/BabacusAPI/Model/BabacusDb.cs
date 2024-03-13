using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BabacusAPI.Model
{
    public class BabacusDb : DbContext
    {
        public BabacusDb(DbContextOptions<BabacusDb> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }

        public DbSet<Invoice> Invoices { get; set; }
    }
}
