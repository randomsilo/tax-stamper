using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using tax_stamper.api.model;
using tax_stamper.domain.model;
using tax_stamper.domain.repository;
using tax_stamper.domain.service;
using tax_stamper.infrastructure.repository;
using tax_stamper.infrastructure.service;


namespace tax_stamper.api.Controllers
{
    [ApiController]
    public class FindTaxRatesController : ControllerBase
    {
        private ILogger _logger;
        private IFindTaxRatesUSA _findTaxRatesUSA;
        private IFindTaxRatesCA _findTaxRatesCA;

        public FindTaxRatesController() : base()
        {
            string INSTANCE_PATH = Environment.GetEnvironmentVariable("INSTANCE_PATH") ?? "dev/dawson1/taxstamper";
            string NAME = this.GetType().Name;
            _logger = GetLogger(INSTANCE_PATH, NAME);

            _findTaxRatesUSA = new FindTaxRatesUSAImpl(
                _logger
                , GetUseTaxRatesRepositoryUSA(_logger, "UseTaxUSA", INSTANCE_PATH)
                , GetSalesTaxRatesRepositoryUSA(_logger, "SalesTaxUSA", INSTANCE_PATH)
            );

            _findTaxRatesCA = new FindTaxRatesCAImpl(
                _logger
                , GetUseTaxRatesRepositoryCA(_logger, "UseTaxCA", INSTANCE_PATH)
                , GetSalesTaxRatesRepositoryCA(_logger, "SalesTaxCA", INSTANCE_PATH)
            );
        }

        [HttpPost("api/usetaxratesusa")]
        public FindTaxRatesResponseUSA GetUseTaxRatesUSA([FromBody]FindTaxRatesRequestUSA searchRequest)
        {
            var response = new FindTaxRatesResponseUSA()
            {
                SearchCriteria = searchRequest.SearchCriteria
                , SearchResults = new TaxResultsUSA()
                , ApiSuccess = true
                , ApiMessage = ""
            };

            try
            {
                response.SearchResults = _findTaxRatesUSA.FindUseTaxRates(searchRequest.SearchCriteria);
            }
            catch(KeyNotFoundException keyNotFound)
            {
                response.ApiSuccess = false;
                response.ApiMessage = keyNotFound.Message;
                _logger.Error(keyNotFound, "GetUseTaxRatesUSA");
            }
            catch(ArgumentException argumentException)
            {
                response.ApiSuccess = false;
                response.ApiMessage = argumentException.Message;
                _logger.Error(argumentException, "GetUseTaxRatesUSA");
            }
            catch(Exception exception)
            {
                response.ApiSuccess = false;
                response.ApiMessage = "Service error.  Please contact support.";
                _logger.Error(exception, "GetUseTaxRatesUSA");
            }
            
            return response;
        }

        [HttpPost("api/salestaxratesusa")]
        public FindTaxRatesResponseUSA GetSalesTaxRatesUSA([FromBody]FindTaxRatesRequestUSA searchRequest)
        {
            var response = new FindTaxRatesResponseUSA()
            {
                SearchCriteria = searchRequest.SearchCriteria
                , SearchResults = new TaxResultsUSA()
                , ApiSuccess = true
                , ApiMessage = ""
            };

            try
            {
                response.SearchResults = _findTaxRatesUSA.FindSalesTaxRates(searchRequest.SearchCriteria);
            }
            catch(KeyNotFoundException keyNotFound)
            {
                response.ApiSuccess = false;
                response.ApiMessage = keyNotFound.Message;
                _logger.Error(keyNotFound, "GetSalesTaxRatesUSA");
            }
            catch(ArgumentException argumentException)
            {
                response.ApiSuccess = false;
                response.ApiMessage = argumentException.Message;
                _logger.Error(argumentException, "GetSalesTaxRatesUSA");
            }
            catch(Exception exception)
            {
                response.ApiSuccess = false;
                response.ApiMessage = "Service error.  Please contact support.";
                _logger.Error(exception, "GetSalesTaxRatesUSA");
            }
            
            return response;
        }

        [HttpPost("api/usetaxratesca")]
        public FindTaxRatesResponseCA GetUseTaxRatesCA([FromBody]FindTaxRatesRequestCA searchRequest)
        {
            var response = new FindTaxRatesResponseCA()
            {
                SearchCriteria = searchRequest.SearchCriteria
                , SearchResults = new TaxResultsCA()
                , ApiSuccess = true
                , ApiMessage = ""
            };

            try
            {
                response.SearchResults = _findTaxRatesCA.FindUseTaxRates(searchRequest.SearchCriteria);
            }
            catch(KeyNotFoundException keyNotFound)
            {
                response.ApiSuccess = false;
                response.ApiMessage = keyNotFound.Message;
                _logger.Error(keyNotFound, "GetUseTaxRatesCA");
            }
            catch(ArgumentException argumentException)
            {
                response.ApiSuccess = false;
                response.ApiMessage = argumentException.Message;
                _logger.Error(argumentException, "GetUseTaxRatesCA");
            }
            catch(Exception exception)
            {
                response.ApiSuccess = false;
                response.ApiMessage = "Service error.  Please contact support.";
                _logger.Error(exception, "GetUseTaxRatesCA");
            }
            
            return response;
        }

        [HttpPost("api/salestaxratesca")]
        public FindTaxRatesResponseCA GetSalesTaxRatesCA([FromBody]FindTaxRatesRequestCA searchRequest)
        {
            var response = new FindTaxRatesResponseCA()
            {
                SearchCriteria = searchRequest.SearchCriteria
                , SearchResults = new TaxResultsCA()
                , ApiSuccess = true
                , ApiMessage = ""
            };

            try
            {
                response.SearchResults = _findTaxRatesCA.FindSalesTaxRates(searchRequest.SearchCriteria);
            }
            catch(KeyNotFoundException keyNotFound)
            {
                response.ApiSuccess = false;
                response.ApiMessage = keyNotFound.Message;
                _logger.Error(keyNotFound, "GetSalesTaxRatesCA");
            }
            catch(ArgumentException argumentException)
            {
                response.ApiSuccess = false;
                response.ApiMessage = argumentException.Message;
                _logger.Error(argumentException, "GetSalesTaxRatesCA");
            }
            catch(Exception exception)
            {
                response.ApiSuccess = false;
                response.ApiMessage = "Service error.  Please contact support.";
                _logger.Error(exception, "GetSalesTaxRatesCA");
            }
            
            return response;
        }

        private ILogger GetLogger(string instancePath, string name)
        {
            string LOGPATH = Environment.GetEnvironmentVariable("LOGPATH") ?? "/opt/data";
            string directory = $"{LOGPATH}/{instancePath}";

            System.IO.Directory.CreateDirectory(directory);

            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.File($"{directory}/{name}.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        private ITaxRatesRepositoryUSA GetUseTaxRatesRepositoryUSA(ILogger logger, string name, string instancePath)
        {
            ITaxRatesRepositoryUSA repository = new SqliteTaxRatesRepositoryUSA(logger, name, instancePath);

            return repository;
        }

        private ITaxRatesRepositoryUSA GetSalesTaxRatesRepositoryUSA(ILogger logger, string name, string instancePath)
        {
            ITaxRatesRepositoryUSA repository = new SqliteTaxRatesRepositoryUSA(logger, name, instancePath);

            return repository;
        }

        private ITaxRatesRepositoryCA GetUseTaxRatesRepositoryCA(ILogger logger, string name, string instancePath)
        {
            ITaxRatesRepositoryCA repository = new SqliteTaxRatesRepositoryCA(logger, name, instancePath);

            return repository;
        }

        private ITaxRatesRepositoryCA GetSalesTaxRatesRepositoryCA(ILogger logger, string name, string instancePath)
        {
            ITaxRatesRepositoryCA repository = new SqliteTaxRatesRepositoryCA(logger, name, instancePath);

            return repository;
        }

    }
}
