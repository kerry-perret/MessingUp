module Tests

open System
open System.IO

open Dapper

open Npgsql

open Xunit

open Microsoft.Data.SqlClient
open Microsoft.Extensions.Configuration

open MessingUp


type ConnectionStrings =
    { SqlServer: string
      PostgreSql: string }

let connectionStrings =
    let getEnvironmentName() = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    let getConnectionStrings() =
        let configurationBuilder =
            ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile(sprintf "appsettings.%s.json" (getEnvironmentName()), true, true)
                .AddEnvironmentVariables()
                .Build()
        { SqlServer = configurationBuilder.GetConnectionString("sqlServer")
          PostgreSql = configurationBuilder.GetConnectionString("postgreSql") }
    lazy(getConnectionStrings())


[<Fact>]
let ``This useless unit test should work!`` () =
    let name = "michelle"
    let actual = Say.hello name
    let expected = sprintf "Hello %s" name
    Assert.Equal(expected, actual)

[<Fact>]
let ``Connect to SQL Server``() =
    use sqlServerConnection = new SqlConnection(connectionStrings.Value.SqlServer)
    let query = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'"
    let actual = sqlServerConnection.ExecuteScalar<int32>(query)
    let expected = 5
    Assert.Equal(expected, actual)

[<Fact>]
let ``Connect to PostgreSQL``() =
    use postgreSqlConnection = new NpgsqlConnection(connectionStrings.Value.PostgreSql)
    let query = "SELECT COUNT(*) from information_schema.tables"
    let actual = postgreSqlConnection.ExecuteScalar<int32>(query)
    let expected = 194
    Assert.Equal(expected, actual)
