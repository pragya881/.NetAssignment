using System;

namespace lifeInsurance.Model
{
    /// <summary>
    /// Contract class to save customer data
    /// </summary>
    public class LifeInsuranceContract
    {
        public string ID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAdrdess { get; set; }
        public string Country { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime SaleDate { get; set; }
        public string CoveragePlan { get; set; }
        public double price { get; set; }
    }
}
