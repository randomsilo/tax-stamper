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
    public class SqliteTaxRatesRepositoryCA : ITaxRatesRepositoryCA
    {
        private ILogger _logger;
        private string _databaseFile { get; set; }
        public SqliteTaxRatesRepositoryCA(ILogger logger, string name, string instanceDirectory, string baseDirectory = @"/opt/data")
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

        public long Create(TaxRateCA record)
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
        public long Delete(TaxRateCA record)
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
        public long Update(TaxRateCA record)
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
        public TaxRateCA FetchById(long id)
        {
            _logger.Verbose($"{this.GetType().Name} IN FetchById");

            var model = new TaxRateCA();

            using (var connection = GetDatabaseConnection())
            {
                connection.Open();
                model = connection.Query<TaxRateCA>(GetFetchByIdStatement(), new { Id = id }).FirstOrDefault();
            }

            _logger.Verbose($"{this.GetType().Name} OUT FetchById");
            return model;
        }
        public TaxRateCA FetchByZipcode(string ForwardStationArea, string localDeliveryUnit, DateTime onDate)
        {
            _logger.Verbose($"{this.GetType().Name} IN FetchByZipcode");

            var model = new TaxRateCA();

            using (var connection = GetDatabaseConnection())
            {
                connection.Open();
                model = connection.Query<TaxRateCA>(
                    GetFetchByZipcodeStatement()
                    , new { 
                        ForwardStationArea = ForwardStationArea
                        , LocalDeliveryUnit = localDeliveryUnit
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
                    @"CREATE TABLE TaxRateCA
                    (
                        Id                      INTEGER PRIMARY KEY AUTOINCREMENT
                        , ForwardStationArea        TEXT NOT NULL
                        , LocalDeliveryUnit     TEXT NOT NULL
                        , EffectiveDate         DATETIME NOT NULL
                        , TaxRateGST            REAL NOT NULL
                        , TaxRatePST            REAL NOT NULL
                        , TaxRateHST            REAL NOT NULL
                    );
                    CREATE INDEX idx_taxrateca_searchkey ON TaxRateCA (ForwardStationArea, LocalDeliveryUnit, EffectiveDate);
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
            var sql = @"INSERT INTO TaxRateCA ( 
                            ForwardStationArea
                            , LocalDeliveryUnit
                            , EffectiveDate
                            , TaxRateGST
                            , TaxRatePST 
                            , TaxRateHST 
                        ) VALUES ( 
                            @ForwardStationArea
                            , @LocalDeliveryUnit
                            , @EffectiveDate
                            , @TaxRateGST
                            , @TaxRatePST
                            , @TaxRateHST
                        );
                        SELECT last_insert_rowid();";

            _logger.Verbose(sql);
            return sql;
        }

        private string GetDeleteStatement()
        {
            _logger.Verbose($"{this.GetType().Name} IN GetDeleteStatement");
            var sql = $"DELETE FROM TaxRateCA WHERE Id = @Id;";

            _logger.Verbose(sql);
            return sql;
        }

        private string GetUpdateStatement()
        {
            _logger.Verbose($"{this.GetType().Name} IN GetUpdateStatement");
            var sql = @"UPDATE TaxRateCA
                        SET
                            ForwardStationArea = @ForwardStationArea
                            , LocalDeliveryUnit = @LocalDeliveryUnit
                            , EffectiveDate = @EffectiveDate
                            , TaxRateGST = @TaxRateGST
                            , TaxRatePST = @TaxRatePST 
                            , TaxRateHST = @TaxRateHST 
                        WHERE Id = @Id;";

            _logger.Verbose(sql);
            return sql;
        }

        private string GetFetchByIdStatement()
        {
            _logger.Verbose($"{this.GetType().Name} IN GetFetchByIdStatement");
            var sql = @"SELECT
                            Id
                            , ForwardStationArea
                            , LocalDeliveryUnit
                            , datetime(EffectiveDate,'unixepoch') AS EffectiveDate
                            , TaxRateGST
                            , TaxRatePST 
                            , TaxRateHST 
                        FROM TaxRateCA WHERE Id = @Id;";

            _logger.Verbose(sql);
            return sql;
        }

        private string GetFetchByZipcodeStatement()
        {
            _logger.Verbose($"{this.GetType().Name} IN GetFetchByZipcodeStatement");
            var sql =  @"SELECT 
                            Id
                            , ForwardStationArea
                            , LocalDeliveryUnit
                            , datetime(EffectiveDate,'unixepoch') AS EffectiveDate
                            , TaxRateGST
                            , TaxRatePST 
                            , TaxRateHST 
                        FROM 
                            TaxRateCA 
                        WHERE 
                            ForwardStationArea = @ForwardStationArea
                            AND LocalDeliveryUnit = @LocalDeliveryUnit
                            AND EffectiveDate <= @EffectiveDate
                        ORDER BY
                            EffectiveDate
                        LIMIT 1;";

            _logger.Verbose(sql);
            return sql;
        }

    }
}
