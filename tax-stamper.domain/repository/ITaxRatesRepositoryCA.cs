using System;
using tax_stamper.domain.entity;

namespace tax_stamper.domain.repository
{
    public interface ITaxRatesRepositoryCA
    {
        long Create(TaxRateCA record);
        long Delete(TaxRateCA record);
        long Update(TaxRateCA record);
        TaxRateCA FetchById(long id);
        TaxRateCA FetchByZipcode(string ForwardStation, string LocalDeliveryUnit);
    }
}