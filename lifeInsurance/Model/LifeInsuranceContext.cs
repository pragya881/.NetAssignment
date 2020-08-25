using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lifeInsurance.Model
{
    public class LifeInsuranceContext: DbContext
    {
        public LifeInsuranceContext(DbContextOptions<LifeInsuranceContext> options)
           : base(options)
        {
        }

        public DbSet<LifeInsuranceContract> LifeInsuranceContract { get; set; }
    }
}
