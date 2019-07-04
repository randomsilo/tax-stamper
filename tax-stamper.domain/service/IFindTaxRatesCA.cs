using System;
using tax_stamper.domain.model;

namespace tax_stamper.domain.service
{
    public interface IFindTaxRatesCA
    {
        TaxResultsCA FindUseTaxRates(TaxSearchCA searchBy);
        TaxResultsCA FindSalesTaxRates(TaxSearchCA searchBy);
    }
}