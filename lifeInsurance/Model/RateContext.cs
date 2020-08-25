using Microsoft.EntityFrameworkCore;

namespace lifeInsurance.Model
{
    public class RateContext : DbContext
    {
        public RateContext(DbContextOptions<RateContext> options)
           : base(options)
        {
        }

        public DbSet<RateContract> RateContract { get; set; }
    }
}
