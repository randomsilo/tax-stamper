# Tax Stamper

Micro service that determine taxes for a given postal area.

## Overview

The goal is to create a asp core web api micro service that takes in some postal data and returns jurisdiction level taxes.

## Outline

- [x] Basics
  - [x] Readme 
  - [x] Changelog
  - [x] File System
- [x] Project Structure
  - [x] Api
  - [x] Domain
  - [x] Domain Test
  - [x] Infrastructure
  - [x] Infrastructure Test
  - [x] Utility (Console Application)
- [ ] Tax Stamper
  - [ ] Api
    - [ ] Routes
      - [x] Business Value
        - [x] GetUseTaxRatesUSA
          - [x] Inputs: zip code plus 4, date
          - [x] Outputs: state, county, city, local 1, and local 2 tax rates
        - [x] GetSalesTaxRatesUSA
          - [x] Inputs: zip code plus 4, date
          - [x] Outputs: state, county, city, local 1, and local 2 tax rates
        - [x] GetUseTaxRatesCA
          - [x] Inputs: forward station, local delivery unit, date
          - [x] Outputs: GST, PST, HST tax rates
        - [x] GetSalesTaxRatesCA
          - [x] Inputs: forward station, local delivery unit, date
          - [x] Outputs: GST, PST, HST tax rates
      - [ ] Dev Ops
        - [ ] Health Check
          - [ ] Inputs: Auth Token
          - [ ] Outputs: cpu load, memory used, memory free, disk used disk free, active threads
        - [ ] IsReady
          - [ ] Outputs: application name, version
    - [ ] Authentication
      - [ ] Simple Header Check
  - [x] Domain
    - [x] Model
      - [x] TaxSearchUSA
      - [x] TaxSearchCA
      - [x] TaxResultsUSA
      - [x] TaxResultsCA
      - [x] Validation
    - [x] Repository
      - [x] ITaxRatesRepositoryUSA
      - [x] ITaxRatesRepositoryCA
    - [x] Service
      - [x] IFindTaxRatesUSA
      - [x] IFindTaxRatesCA
  - [x] Infrastructure
    - [x] Repository
      - [x] SqliteTaxRatesRepositoryUSA
      - [x] SqliteTaxRatesRepositoryCA
    - [x] Service
      - [x] FindTaxRatesUSAImpl
      - [x] FindTaxRatesCAImpl

## References

[Postal codes in Canada](https://en.wikipedia.org/wiki/Postal_codes_in_Canada)