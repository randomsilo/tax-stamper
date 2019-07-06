using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace tax_stamper.api.Controllers
{
    [ApiController]
    public class FindTaxRatesController : ControllerBase
    {

        [HttpGet("api/usetaxratesusa")]
        public void GetUseTaxRatesUSA()
        {
            
        }

    }
}
