using gRPC_for_ASPNET_CORE.Model;
using Microsoft.EntityFrameworkCore;

namespace gRPC_for_ASPNET_CORE.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {


        }

        public DbSet<Product> Products { get; set; }

    }
}
