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
  - [x] Api Test
  - [x] Domain
  - [x] Domain Test
  - [x] Infrastructure
  - [x] Infrastructure Test
  - [x] Utility (Console Application)
- [ ] Tax Stamper
  - [ ] Api
    - [ ] Routes
      - [ ] Business Value
        - [ ] GetUseTaxRatesUSA
          - [ ] Inputs: zip code plus 4, date
          - [ ] Outputs: state, county, city, local 1, and local 2 tax rates
        - [ ] GetSalesTaxRatesUSA
          - [ ] Inputs: zip code plus 4, date
          - [ ] Outputs: state, county, city, local 1, and local 2 tax rates
        - [ ] GetUseTaxRatesCA
          - [ ] Inputs: forward station, local delivery unit, date
          - [ ] Outputs: GST, PST, HST tax rates
        - [ ] GetSalesTaxRatesCA
          - [ ] Inputs: forward station, local delivery unit, date
          - [ ] Outputs: GST, PST, HST tax rates
      - [ ] Dev Ops
        - [ ] Health Check
          - [ ] Inputs: Auth Token
          - [ ] Outputs: cpu load, memory used, memory free, disk used disk free, active threads
        - [ ] IsReady
          - [ ] Outputs: application name, version
    - [ ] Authentication
      - [ ] Simple Header Check
  - [ ] Domain
    - [ ] Model
      - [ ] TaxSearchUSA
      - [ ] TaxSearchCA
      - [ ] TaxResultsUSA
      - [ ] TaxResultsCA
    - [ ] Repository
      - [ ] ITaxRatesRepositoryUSA
      - [ ] ITaxRatesRepositoryCA
    - [ ] Service
      - [ ] IFindTaxRatesUSA
      - [ ] IFindTaxRatesCA
  - [ ] Infrastructure
    - [ ] Repository
      - [ ] SqliteTaxRatesRepositoryUSA
      - [ ] SqliteTaxRatesRepositoryCA
    - [ ] Service
      - [ ] FindTaxRatesUSAImpl
      - [ ] FindTaxRatesCAImpl

## References

[Postal codes in Canada](https://en.wikipedia.org/wiki/Postal_codes_in_Canada)