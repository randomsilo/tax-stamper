using System;
using tax_stamper.domain.model;



namespace tax_stamper.api.model
{
    public class FindTaxRatesRequestUSA
    {
        public TaxSearchUSA SearchCriteria { get; set; }
    }
}