using System;
using tax_stamper.domain.entity;

namespace tax_stamper.domain.repository
{
    public interface ITaxRatesRepositoryUSA
    {
        long Create(TaxRateUSA record);
        long Delete(TaxRateUSA record);
        long Update(TaxRateUSA record);
        TaxRateUSA FetchById(long id);
        TaxRateUSA FetchByZipcode(int zipcode, int zipPlus4);
    }
}