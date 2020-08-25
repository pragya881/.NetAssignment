using lifeInsurance.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace lifeInsurance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LifeInsuranceContractsController : ControllerBase
    {
        private readonly LifeInsuranceContext _lifeInsuranceContext;
        private readonly CoveragePlanContext  _coveragePlanContext;
        private readonly RateContext _rateContext;

        public LifeInsuranceContractsController(LifeInsuranceContext liContext, CoveragePlanContext coveragePlanContext, RateContext rateContext)
        {
            _lifeInsuranceContext = liContext;
            _coveragePlanContext = coveragePlanContext;
            _rateContext = rateContext;
        }

       /// <summary>
       /// Get all the life insurance contract
       /// </summary>
       /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LifeInsuranceContract>>> GetLifeInsuranceContract()
        {
            return await _lifeInsuranceContext.LifeInsuranceContract.ToListAsync();
        }

       /// <summary>
       /// Create new life insurance contract
       /// </summary>
       /// <param name="lifeInsuranceContract"></param>
       /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<LifeInsuranceContract>> PostLifeInsuranceContract(LifeInsuranceContract lifeInsuranceContract)
        {
            var LiContract = EvaluatePlan(lifeInsuranceContract);
            if (LiContract == null)
            {
                return NotFound();
            }
            try
            {
                _lifeInsuranceContext.LifeInsuranceContract.Add(LiContract);
                await _lifeInsuranceContext.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return CreatedAtAction("GetLifeInsuranceContract", new { id = LiContract.ID }, LiContract);
        }

        /// <summary>
        /// Update the life insurance contract
        /// </summary>
        /// <param name="id"></param>
        /// <param name="lifeInsuranceContract"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLifeInsuranceContract(string id, LifeInsuranceContract lifeInsuranceContract)
        {
            if (id != lifeInsuranceContract.ID)
            {
                return BadRequest();
            }

            _lifeInsuranceContext.Entry(lifeInsuranceContract).State = EntityState.Modified;

            try
            {
                await _lifeInsuranceContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LifeInsuranceContractExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }        

      
        /// <summary>
        /// Delete particular life insurance
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<LifeInsuranceContract>> DeleteLifeInsuranceContract(string id)
        {
            var lifeInsuranceContract = await _lifeInsuranceContext.LifeInsuranceContract.FindAsync(id);
            if (lifeInsuranceContract == null)
            {
                return NotFound();
            }

            _lifeInsuranceContext.LifeInsuranceContract.Remove(lifeInsuranceContract);
            await _lifeInsuranceContext.SaveChangesAsync();

            return lifeInsuranceContract;
        }

       
        private LifeInsuranceContract EvaluatePlan(LifeInsuranceContract lifeInsurance)
        {
            var age = CalculateAge(lifeInsurance.DateOfBirth);
            var coveragePlane = _coveragePlanContext.CoveragePlanContract.Where(c =>
                                 c.Country == lifeInsurance.Country && (lifeInsurance.SaleDate >= c.StartDate && lifeInsurance.SaleDate <= c.EndDate)
                                 );
            var rateList = _rateContext.RateContract.Where(r => r.CustomerGender == lifeInsurance.Gender).Join(coveragePlane,
                             rate => rate.CoveragePlan,
                             coverage => coverage.CoveragePlan,
                             (rate, covergae) => new { rate });

            var rate = rateList.Where(r => evaluatePrice(r.rate.Age.Replace("40", ""), age)).SingleOrDefault();

            lifeInsurance.CoveragePlan = rate.rate.CoveragePlan;
            return lifeInsurance;
        }

        private int CalculateAge(DateTime DoB)
        {
            var today = DateTime.Today;
            var age = today.Year - DoB.Year;
            if (DoB.Date > today.AddYears(-age)) age--;
            return age;
        }

        private bool evaluatePrice(string logic, int age)
        {
            switch (logic)
            {
                case ">": return 40 > age;
                case "<=": return 40 <= age;
            }
            return false;
        }
        private bool LifeInsuranceContractExists(string id)
        {
            return _lifeInsuranceContext.LifeInsuranceContract.Any(e => e.ID == id);
        }
    }
}
