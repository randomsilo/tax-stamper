using System;
using tax_stamper.domain.model;

namespace tax_stamper.domain.service
{
    public interface IFindTaxRatesUSA
    {
        TaxResultsUSA FindUseTaxRates(TaxSearchUSA searchBy);
        TaxResultsUSA FindSalesTaxRates(TaxSearchUSA searchBy);
    }
}