using System;

namespace tax_stamper.domain.model
{
    public class TaxSearchCA
    {
        public string ForwardStation { get; set; }
        public string LocalDeliveryUnit { get; set; }
        public DateTime OnDate { get; set; }
    }
}
