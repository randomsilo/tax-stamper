# Dot Net Commands

The dot net cli commands used to setup the project are documented here.
Please update this file with whatever commands are used to update a solution or project.
Documenting the commands used will be a huge time saving as new microservices are developed.  

## File System

```bash
mkdir -p ./tax-stamper.api
mkdir -p ./tax-stamper.domain
mkdir -p ./tax-stamper.domain.test
mkdir -p ./tax-stamper.infrastructure
mkdir -p ./tax-stamper.infrastructure.test
mkdir -p ./tax-stamper.utility
```

## Projects

```bash
# make solution inside project base directory
dotnet new sln

# make directory for each project and initialize the project
cd ./tax-stamper.api
dotnet new webapi
cd ..

cd ./tax-stamper.domain
dotnet new classlib
cd ..

cd ./tax-stamper.domain.test
dotnet new xunit
cd ..

cd ./tax-stamper.infrastructure
dotnet new classlib
cd ..

cd ./tax-stamper.infrastructure.test
dotnet new xunit
cd ..

cd ./tax-stamper.utility
dotnet new console
cd ..

# add projects to solution
dotnet sln tax-stamper.sln add tax-stamper.api/tax-stamper.api.csproj
dotnet sln tax-stamper.sln add tax-stamper.domain/tax-stamper.domain.csproj
dotnet sln tax-stamper.sln add tax-stamper.domain.test/tax-stamper.domain.test.csproj
dotnet sln tax-stamper.sln add tax-stamper.infrastructure/tax-stamper.infrastructure.csproj
dotnet sln tax-stamper.sln add tax-stamper.infrastructure.test/tax-stamper.infrastructure.test.csproj
dotnet sln tax-stamper.sln add tax-stamper.utility/tax-stamper.utility.csproj

## add internal references
dotnet add tax-stamper.api/tax-stamper.api.csproj reference tax-stamper.domain/tax-stamper.domain.csproj
dotnet add tax-stamper.api/tax-stamper.api.csproj reference tax-stamper.infrastructure/tax-stamper.infrastructure.csproj

dotnet add tax-stamper.domain.test/tax-stamper.domain.test.csproj reference tax-stamper.domain/tax-stamper.domain.csproj

dotnet add tax-stamper.infrastructure/tax-stamper.infrastructure.csproj reference tax-stamper.domain/tax-stamper.domain.csproj
dotnet add tax-stamper.infrastructure.test/tax-stamper.infrastructure.test.csproj reference tax-stamper.domain/tax-stamper.domain.csproj

dotnet add tax-stamper.utility/tax-stamper.utility.csproj reference tax-stamper.infrastructure/tax-stamper.infrastructure.csproj
dotnet add tax-stamper.utility/tax-stamper.utility.csproj reference tax-stamper.domain/tax-stamper.domain.csproj

# add external dependencies
cd ./tax-stamper.api
dotnet add package Dapper
dotnet add package Serilog
dotnet add package Serilog.Sinks.Console
dotnet add package Serilog.Sinks.File
cd ..

cd ./tax-stamper.domain
dotnet add package Dapper
dotnet add package Serilog
cd ..

cd ./tax-stamper.domain.test
dotnet add package Bogus
dotnet add package Serilog
dotnet add package Serilog.Sinks.Console
dotnet add package Serilog.Sinks.File
cd ..

cd ./tax-stamper.infrastructure
dotnet add package System.Data.SQLite
dotnet add package Dapper
dotnet add package Serilog
cd ..

cd ./tax-stamper.infrastructure.test
dotnet add package Bogus
dotnet add package Serilog
dotnet add package Serilog.Sinks.Console
dotnet add package Serilog.Sinks.File
cd ..

cd ./tax-stamper.utility
dotnet add package System.Data.SQLite
dotnet add package Dapper
dotnet add package Serilog
dotnet add package Serilog.Sinks.Console
dotnet add package Serilog.Sinks.File
cd ..

```