using System;

namespace tax_stamper.domain.model
{
    public class TaxResultsUSA
    {
        public double TaxRateState { get; set; }
        public double TaxRateCounty { get; set; }
        public double TaxRateCity { get; set; }
        public double TaxRateLocal1 { get; set; }
        public double TaxRateLocal2 { get; set; }
    }
}
