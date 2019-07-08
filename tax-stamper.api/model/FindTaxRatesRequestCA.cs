using System;
using tax_stamper.domain.model;



namespace tax_stamper.api.model
{
    public class FindTaxRatesRequestCA
    {
        public TaxSearchCA SearchCriteria { get; set; }
    }
}