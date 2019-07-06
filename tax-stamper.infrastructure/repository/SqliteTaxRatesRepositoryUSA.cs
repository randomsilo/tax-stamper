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
    public class SqliteTaxRatesRepositoryUSA : ITaxRatesRepositoryUSA
    {
        private ILogger _logger;
        private string _databaseFile { get; set; }
        public SqliteTaxRatesRepositoryUSA(ILogger logger, string name, string instanceDirectory, string baseDirectory = @"/opt/data")
        {
            // logger
            _logger = logger;
            _logger.Verbose($"{this.GetType().Name} IN Constructor");

            string filePath = $"{baseDirectory}/{instanceDirectory}/";
            string fileName = $"{name}.sqlite";
            _databaseFile = $"{filePath}/{fileName}";

            // check for database directory
            if (!System.IO.Directory.Exists(filePath))
            {
                _logger.Verbose($"{this.GetType().Name} missing {filePath}, will try to create directory");
                System.IO.Directory.CreateDirectory(filePath);
            }

            // check for database
            if (!File.Exists(_databaseFile))
            {
                _logger.Verbose($"{this.GetType().Name} missing {_databaseFile}, will try to create database");

                // create database
                CreateDatabase();
            }
        }

        public long Create(TaxRateUSA record)
        {
            _logger.Verbose($"{this.GetType().Name} IN Create");

            long id = 0;

            using (var connection = GetDatabaseConnection())
            {
                connection.Open();
                id = connection.Query<long>(GetInsertStatement(), record).First();
            }

            _logger.Verbose($"{this.GetType().Name} OUT Create, id is {id}");
            return id;
        }
        public long Delete(TaxRateUSA record)
        {
            _logger.Verbose($"{this.GetType().Name} IN Delete");

            long rowsDeleted = 0;

            using (var connection = GetDatabaseConnection())
            {
                connection.Open();
                rowsDeleted = connection.Execute(GetDeleteStatement(), new { Id = record.Id });
            }

            _logger.Verbose($"{this.GetType().Name} OUT Delete, rowsDeleted is {rowsDeleted}");
            return rowsDeleted;
        }
        public long Update(TaxRateUSA record)
        {
            _logger.Verbose($"{this.GetType().Name} IN Update");

            long rowsUpdated = 0;

            using (var connection = GetDatabaseConnection())
            {
                connection.Open();
                rowsUpdated = connection.Execute(GetUpdateStatement(), record);
            }

            _logger.Verbose($"{this.GetType().Name} OUT Delete, rowsUpdated is {rowsUpdated}");
            return rowsUpdated;
        }
        public TaxRateUSA FetchById(long id)
        {
            _logger.Verbose($"{this.GetType().Name} IN FetchById");

            var model = new TaxRateUSA();

            using (var connection = GetDatabaseConnection())
            {
                connection.Open();
                model = connection.Query<TaxRateUSA>(GetFetchByIdStatement(), new { Id = id }).FirstOrDefault();
            }

            _logger.Verbose($"{this.GetType().Name} OUT FetchById");
            return model;
        }
        public TaxRateUSA FetchByZipcode(int zipcode, int zipPlus4, DateTime onDate)
        {
            _logger.Verbose($"{this.GetType().Name} IN FetchByZipcode");

            var model = new TaxRateUSA();

            using (var connection = GetDatabaseConnection())
            {
                connection.Open();
                model = connection.Query<TaxRateUSA>(
                    GetFetchByZipcodeStatement()
                    , new { 
                        Zipcode = zipcode
                        , ZipPlus4 = zipPlus4
                        , EffectiveDate = onDate
                    }).FirstOrDefault();
            }

            _logger.Verbose($"{this.GetType().Name} OUT FetchByZipcode");
            return model;
        }


        private void CreateDatabase()
        {
            _logger.Verbose($"{this.GetType().Name} IN Create");

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
                    );
                    CREATE INDEX idx_taxrateusa_searchkey ON TaxRateUSA (Zipcode, ZipPlus4StartRange, ZipPlus4EndRange, EffectiveDate);
                    ");
                
            }
        }

        public SQLiteConnection GetDatabaseConnection()
        {
            return new SQLiteConnection("Data Source=" + _databaseFile);
        }

        private string GetInsertStatement()
        {
            _logger.Verbose($"{this.GetType().Name} IN GetInsertStatement");
            var sql = @"INSERT INTO TaxRateUSA ( 
                        Zipcode
                        , ZipPlus4StartRange
                        , ZipPlus4EndRange
                        , EffectiveDate
                        , TaxRateState
                        , TaxRateCounty 
                        , TaxRateCity 
                        , TaxRateLocal1 
                        , TaxRateLocal2 
                    ) VALUES ( 
                        @Zipcode
                        , @ZipPlus4StartRange
                        , @ZipPlus4EndRange
                        , @EffectiveDate
                        , @TaxRateState
                        , @TaxRateCounty
                        , @TaxRateCity
                        , @TaxRateLocal1
                        , @TaxRateLocal2
                    );
                    SELECT last_insert_rowid();";

            _logger.Verbose(sql);
            return sql;
        }

        private string GetDeleteStatement()
        {
            _logger.Verbose($"{this.GetType().Name} IN GetDeleteStatement");
            var sql = $"DELETE FROM TaxRateUSA WHERE Id = @Id;";

            _logger.Verbose(sql);
            return sql;
        }

        private string GetUpdateStatement()
        {
            _logger.Verbose($"{this.GetType().Name} IN GetUpdateStatement");
            var sql = @"UPDATE TaxRateUSA
                     SET
                        Zipcode = @Zipcode
                        , ZipPlus4StartRange = @ZipPlus4StartRange
                        , ZipPlus4EndRange = @ZipPlus4EndRange
                        , EffectiveDate = @EffectiveDate
                        , TaxRateState = @TaxRateState
                        , TaxRateCounty = @TaxRateCounty 
                        , TaxRateCity = @TaxRateCity 
                        , TaxRateLocal1 = @TaxRateLocal1 
                        , TaxRateLocal2 = @TaxRateLocal2 
                    WHERE Id = @Id;";

            _logger.Verbose(sql);
            return sql;
        }

        private string GetFetchByIdStatement()
        {
            _logger.Verbose($"{this.GetType().Name} IN GetFetchByIdStatement");
            var sql = @"SELECT
                        Id
                        , Zipcode
                        , ZipPlus4StartRange
                        , ZipPlus4EndRange
                        , datetime(EffectiveDate,'unixepoch') AS EffectiveDate
                        , TaxRateState
                        , TaxRateCounty 
                        , TaxRateCity 
                        , TaxRateLocal1 
                        , TaxRateLocal2 
                    FROM TaxRateUSA WHERE Id = @Id;";

            _logger.Verbose(sql);
            return sql;
        }

        private string GetFetchByZipcodeStatement()
        {
            _logger.Verbose($"{this.GetType().Name} IN GetFetchByZipcodeStatement");
            var sql =  @"SELECT 
                        Id
                        , Zipcode
                        , ZipPlus4StartRange
                        , ZipPlus4EndRange
                        , datetime(EffectiveDate,'unixepoch') AS EffectiveDate
                        , TaxRateState
                        , TaxRateCounty 
                        , TaxRateCity 
                        , TaxRateLocal1 
                        , TaxRateLocal2 
                    FROM 
                        TaxRateUSA 
                    WHERE 
                        Zipcode = @Zipcode
                        AND ZipPlus4StartRange <= @ZipPlus4
                        AND ZipPlus4EndRange >= @ZipPlus4
                        AND EffectiveDate <= @EffectiveDate
                    ORDER BY
                        EffectiveDate, Id desc
                    LIMIT 1;";

            _logger.Verbose(sql);
            return sql;
        }

    }
}
