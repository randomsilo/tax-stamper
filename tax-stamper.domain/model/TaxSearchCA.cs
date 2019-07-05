using System;

namespace tax_stamper.domain.model
{
    public class TaxSearchCA
    {
        public string ForwardStationArea { get; set; }
        public string LocalDeliveryUnit { get; set; }
        public DateTime OnDate { get; set; }

        public (bool validStatus, string whyNot) IsValid()
        {
            bool validStatus = true;
            string whyNot = "";

            do {

                if (OnDate > new DateTime(2099,1,1))
                {
                    validStatus = false;
                    whyNot = string.Format(
                        "OnDate ({0}) is after the 2099"
                        , OnDate.ToString("MM/dd/yyyy")
                    );
                    break;
                }

            } while(false);

            return (validStatus, whyNot);
        }
    }
}
