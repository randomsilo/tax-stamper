using System;

namespace tax_stamper.domain.model
{
    public class TaxResultsCA
    {
        public double TaxRateGST { get; set; }
        public double TaxRatePST { get; set; }
        public double TaxRateHST { get; set; }
    }
}
