using FCamara.CommissionCalculator.Models;
using FCamara.CommissionCalculator.Options;
using Microsoft.Extensions.Options;

namespace FCamara.CommissionCalculator.Services
{
    public sealed class CommissionCalculator : ICommissionCalculator
    {
        private readonly CommissionOptions _opts;

        public CommissionCalculator(IOptions<CommissionOptions> opts)
        {
            _opts = opts?.Value ?? new CommissionOptions();
        }

        public CommissionCalculationResponse Calculate(CommissionCalculationRequest request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));
            if (request.LocalSalesCount < 0 || request.ForeignSalesCount < 0 || request.AverageSaleAmount < 0)
                throw new ArgumentOutOfRangeException("Inputs must be non-negative.");

            decimal localBase = request.LocalSalesCount * request.AverageSaleAmount;
            decimal foreignBase = request.ForeignSalesCount * request.AverageSaleAmount;

            decimal fcamara = (_opts.FCamaraLocalRate * localBase) + (_opts.FCamaraForeignRate * foreignBase);
            decimal competitor = (_opts.CompetitorLocalRate * localBase) + (_opts.CompetitorForeignRate * foreignBase);

            fcamara = Math.Round(fcamara, 2, MidpointRounding.AwayFromZero);
            competitor = Math.Round(competitor, 2, MidpointRounding.AwayFromZero);

            return new CommissionCalculationResponse
            {
                FCamaraCommissionAmount = fcamara,
                CompetitorCommissionAmount = competitor
            };
        }
    }
}
