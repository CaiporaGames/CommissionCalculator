using FCamara.CommissionCalculator.Models;

namespace FCamara.CommissionCalculator.Services
{
    public interface ICommissionCalculator
    {
        CommissionCalculationResponse Calculate(CommissionCalculationRequest request);
    }
}
