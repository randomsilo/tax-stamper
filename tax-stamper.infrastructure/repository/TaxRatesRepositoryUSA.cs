using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data.SQLite;
using System.IO;
using Dapper;
using Serilog;
using tax_stamper.domain.entity;
using tax_stamper.domain.repository;

namespace tax_stamper.infrastructure.repository
{
    public class TaxRatesRepositoryUSA : ITaxRatesRepositoryUSA
    {
        private string _databaseFile { get; set; }
        public TaxRatesRepositoryUSA(string name, string instanceDirectory, string baseDirectory = @"/opt/data")
        {
            string filePath = $"{baseDirectory}/{instanceDirectory}/";
            string fileName = $"{name}.sqlite";
            _databaseFile = $"{filePath}/{fileName}";

            // Check for Database Directory
            if (!System.IO.Directory.Exists(filePath))
            {
                System.IO.Directory.CreateDirectory(filePath);
            }

            // Check for Database
            if (!File.Exists(_databaseFile))
            {
                // Create Database
                CreateDatabase();
            }
        }

        public long Create(TaxRateUSA record)
        {
            long id = 0;

            return id;
        }
        public long Delete(TaxRateUSA record)
        {
            long rowsDeleted = 0;

            return rowsDeleted;
        }
        public long Update(TaxRateUSA record)
        {
            long rowsUpdated = 0;

            return rowsUpdated;
        }
        public TaxRateUSA FetchById(long id)
        {
            var model = new TaxRateUSA();

            return model;
        }
        public TaxRateUSA FetchByZipcode(int zipcode, int zipPlus4)
        {
            var model = new TaxRateUSA();

            return model;
        }


        private void CreateDatabase()
        {
            using (var connection = GetDatabaseConnection())
            {
                connection.Open();
                connection.Execute(
                    @"CREATE TABLE TaxRateUSA
                    (
                        Id                      INTEGER PRIMARY KEY AUTOINCREMENT
                        , Zipcode               INTEGER NOT NULL
                        , ZipPlus4StartRange    INTEGER NOT NULL
                        , ZipPlus4EndRange      INTEGER NOT NULL
                        , EffectiveDate         DATETIME NOT NULL
                        , TaxRateState          REAL NOT NULL
                        , TaxRateCounty         REAL NOT NULL
                        , TaxRateCity           REAL NOT NULL
                        , TaxRateLocal1         REAL NOT NULL
                        , TaxRateLocal2         REAL NOT NULL
                    );");
                
            }
        }

        public SQLiteConnection GetDatabaseConnection()
        {
            return new SQLiteConnection("Data Source=" + _databaseFile);
        }

    }
}
