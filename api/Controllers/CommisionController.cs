using FCamara.CommissionCalculator.Models;
using FCamara.CommissionCalculator.Services;
using Microsoft.AspNetCore.Mvc;

namespace FCamara.CommissionCalculator.Controllers
{
    [ApiController]
    [Route("commission")]
    public class CommissionController : ControllerBase
    {
        private readonly ICommissionCalculator _calculator;

        public CommissionController(ICommissionCalculator calculator)
        {
            _calculator = calculator;
        }

        [ProducesResponseType(typeof(CommissionCalculationResponse), 200)]
        [ProducesResponseType(400)]
        [HttpPost]
        public IActionResult Calculate([FromBody] CommissionCalculationRequest request)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var result = _calculator.Calculate(request);
            return Ok(result);
        }
    }
}
