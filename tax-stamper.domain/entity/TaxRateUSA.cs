using System;

namespace tax_stamper.domain.entity
{
    public class TaxRateUSA
    {
        public long Id { get; set; }
        public int Zipcode { get; set; }
        public int ZipPlus4StartRange { get; set; }
        public int ZipPlus4EndRange { get; set; }
        public DateTime EffectiveDate { get; set; }
        public double TaxRateState { get; set; }
        public double TaxRateCounty { get; set; }
        public double TaxRateCity { get; set; }
        public double TaxRateLocal1 { get; set; }
        public double TaxRateLocal2 { get; set; }
    }
}
