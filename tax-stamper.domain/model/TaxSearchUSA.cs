using System;

namespace tax_stamper.domain.model
{
    public class TaxSearchUSA
    {
        public string Zipcode { get; set; }
        public string ZipPlus4 { get; set; }
        public DateTime OnDate { get; set; }
    }
}
