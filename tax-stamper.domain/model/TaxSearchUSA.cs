using System;

namespace tax_stamper.domain.model
{
    public class TaxSearchUSA
    {
        public string Zipcode { get; set; }
        public string ZipPlus4 { get; set; }
        public DateTime OnDate { get; set; }

        public (bool validStatus, string whyNot) IsValid()
        {
            bool validStatus = true;
            string whyNot = "";
            int zipcode;
            int zipPlus4;

            do {

                if (!int.TryParse(Zipcode, out zipcode))
                {
                    validStatus = false;
                    whyNot = "Zipcode ({Zipcode}) is not numeric";
                    break;
                }

                if (!int.TryParse(ZipPlus4, out zipPlus4))
                {
                    validStatus = false;
                    whyNot = "ZipPlus4 ({ZipPlus4}) is not numeric";
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
