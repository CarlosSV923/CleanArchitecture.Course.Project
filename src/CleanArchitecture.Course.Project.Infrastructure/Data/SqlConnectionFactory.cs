using System.Data;
using CleanArchitecture.Course.Project.Application.Abstractions.Data;
using Npgsql;

namespace CleanArchitecture.Course.Project.Infrastructure.Data
{
    internal sealed class SqlConnectionFactory(string connectionString) : ISqlConnectionFactory
    {
        private readonly string _connectionString = connectionString;

        public IDbConnection CreateConnection()
        {
            var connection = new NpgsqlConnection(_connectionString);

            connection.Open();

            return connection;

        }
    }
}