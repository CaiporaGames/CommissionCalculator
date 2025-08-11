using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace FCamara.CommissionCalculator.Controllers
{
    [ApiController]
    [Route("commission")]
    public class CommissionController : ControllerBase
    {
        private const decimal FCAMARA_LOCAL_RATE = 0.20m;
        private const decimal FCAMARA_FOREIGN_RATE = 0.35m;
        private const decimal COMP_LOCAL_RATE = 0.02m;
        private const decimal COMP_FOREIGN_RATE = 0.0755m;

        [ProducesResponseType(typeof(CommissionCalculationResponse), 200)]
        [ProducesResponseType(400)]
        [HttpPost]
        public IActionResult Calculate([FromBody] CommissionCalculationRequest calculationRequest)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            // Calculate
            decimal localBase = calculationRequest.LocalSalesCount * calculationRequest.AverageSaleAmount;
            decimal foreignBase = calculationRequest.ForeignSalesCount * calculationRequest.AverageSaleAmount;

            decimal fcamaraTotal = (FCAMARA_LOCAL_RATE * localBase) + (FCAMARA_FOREIGN_RATE * foreignBase);
            decimal competitorTotal = (COMP_LOCAL_RATE * localBase) + (COMP_FOREIGN_RATE * foreignBase);

            // Round to cents
            fcamaraTotal = Math.Round(fcamaraTotal, 2, MidpointRounding.AwayFromZero);
            competitorTotal = Math.Round(competitorTotal, 2, MidpointRounding.AwayFromZero);

            return Ok(new CommissionCalculationResponse
            {
                FCamaraCommissionAmount = fcamaraTotal,
                CompetitorCommissionAmount = competitorTotal
            });
        }
    }

    public class CommissionCalculationRequest
    {
        [Range(0, int.MaxValue)]
        public int LocalSalesCount { get; set; }

        [Range(0, int.MaxValue)]
        public int ForeignSalesCount { get; set; }

        [Range(0, double.MaxValue)]
        public decimal AverageSaleAmount { get; set; }
    }

    public class CommissionCalculationResponse
    {
        public decimal FCamaraCommissionAmount { get; set; }
        public decimal CompetitorCommissionAmount { get; set; }
    }
}
