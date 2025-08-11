using System.ComponentModel.DataAnnotations;

namespace FCamara.CommissionCalculator.Models
{
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
