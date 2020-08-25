using System;

namespace lifeInsurance.Model
{
    public class CoveragePlanContract
    {
        public string ID { get; set; }
        public string CoveragePlan { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Country { get; set; }
    }
}
