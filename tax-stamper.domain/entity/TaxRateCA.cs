using System;

namespace tax_stamper.domain.entity
{
    public class TaxRateCA
    {
        public long Id { get; set; }
        public string ForwardStation { get; set; }
        public string LocalDeliveryUnit { get; set; }
        public DateTime EffectiveDate { get; set; }
        public double TaxRateGST { get; set; }
        public double TaxRatePST { get; set; }
        public double TaxRateHST { get; set; }
    }
}
