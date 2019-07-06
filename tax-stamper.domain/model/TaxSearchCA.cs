using System;
using System.Text.RegularExpressions;

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

                // forward station area
                Regex forwardStationAreaRegex = new Regex(@"[A-Z][0-9][A-Z]");
                Match forwardStationAreaMatch = forwardStationAreaRegex.Match(ForwardStationArea);
                if (!forwardStationAreaMatch.Success)
                {
                   validStatus = false;
                    whyNot = string.Format(
                        "ForwardStationArea ({0}) pattern is Letter Number Letter"
                        , ForwardStationArea
                    );
                    break;
                }

                // local delivery unit
                Regex localDeliveryUnitRegex = new Regex(@"[0-9][A-Z][0-9]");
                Match localDeliveryUnitMatch = localDeliveryUnitRegex.Match(LocalDeliveryUnit);
                if (!localDeliveryUnitMatch.Success)
                {
                   validStatus = false;
                    whyNot = string.Format(
                        "LocalDeliveryUnit ({0}) pattern is Number Letter Number"
                        , LocalDeliveryUnit
                    );
                    break;
                }

                if (OnDate < new DateTime(1900,1,1))
                {
                    validStatus = false;
                    whyNot = string.Format(
                        "OnDate ({0}) is before the 1900s"
                        , OnDate.ToString("MM/dd/yyyy")
                    );
                    break;
                }

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
