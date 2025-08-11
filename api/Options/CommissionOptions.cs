namespace FCamara.CommissionCalculator.Options
{
    public sealed class CommissionOptions
    {
        public decimal FCamaraLocalRate { get; init; } = 0.20m;
        public decimal FCamaraForeignRate { get; init; } = 0.35m;
        public decimal CompetitorLocalRate { get; init; } = 0.02m;
        public decimal CompetitorForeignRate { get; init; } = 0.0755m;
    }
}
