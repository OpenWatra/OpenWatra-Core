# WaTra

Application to calculate heavy duty water transport, part of CAS OOP/21.11 @ ZHAW.

## Build

* Install the dotnet Cake tool:
  `dotnet tool install Cake.Tool --version 1.0.0 --global`
* Build using dotnet-cake

## Configuration

### Tests

* SQL Server instance `.\` is required
* Currently logged in user must be able to create a DB where they are the `db_owner`
* Run tests from within the Visual Studio Test Explorer

### Debug

* SQL Server instance `.\` is required
* Currently logged in user must be able to create a DB where they are the `db_owner`
* SQL Server connection string may be modified in `src\Watra.Api\appsettings.json`
* Set `Watra.Api`, `Watra.Client.Desktop` and `Watra.Client.Web` as startup projects
  * Start debugging using `F5`
