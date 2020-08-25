using Microsoft.EntityFrameworkCore;

namespace lifeInsurance.Model
{
    public class CoveragePlanContext: DbContext
    {
        public CoveragePlanContext(DbContextOptions<CoveragePlanContext> options)
           : base(options)
        {
        }

        public DbSet<CoveragePlanContract> CoveragePlanContract { get; set; }
    }
}
