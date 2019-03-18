using System;
using System.Data;

namespace Monytor.Implementation.Collectors.SQL {
    public static class DbConnectionFactory {
        public static IDbConnection CreateDbConnection(SqlDatabaseSource sqlDatabaseSource) {
            switch (sqlDatabaseSource?.DatabaseProvider) {
                case DatabaseProvider.MSSQL:
                    return new System.Data.SqlClient.SqlConnection(sqlDatabaseSource.ConnectionString);
                case DatabaseProvider.PostgreSQL:
                    return new Npgsql.NpgsqlConnection(sqlDatabaseSource.ConnectionString);
                case DatabaseProvider.MySQL:
                    return new MySql.Data.MySqlClient.MySqlConnection(sqlDatabaseSource.ConnectionString);
                case DatabaseProvider.Oracle:
                    return new Oracle.ManagedDataAccess.Client.OracleConnection(sqlDatabaseSource.ConnectionString);
                default:
                    throw new NotSupportedException($"The database provider {sqlDatabaseSource.DatabaseProvider} is not supported.");
            }
        }
    }
}
