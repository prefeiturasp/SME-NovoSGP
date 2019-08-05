using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Data;

namespace SME.SGP.Dados.Contexto
{
    public class DbContext : SgpContext
    {
        private readonly NpgsqlConnection connection;

        public DbContext(IConfiguration configuration)
        {
            connection = new NpgsqlConnection(configuration.GetConnectionString("SGP-Postgres"));
            Open();
        }

        public string ConnectionString { get { return connection.ConnectionString; } set { connection.ConnectionString = value; } }

        public int ConnectionTimeout => connection.ConnectionTimeout;

        public string Database => connection.Database;

        public ConnectionState State => connection.State;

        public IDbTransaction BeginTransaction()
        {
            return connection.BeginTransaction();
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return connection.BeginTransaction(il);
        }

        public void ChangeDatabase(string databaseName)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            connection.Close();
        }

        public NpgsqlConnection Connection() => connection;

        public IDbCommand CreateCommand()
        {
            return connection.CreateCommand();
        }

        public void Dispose() => connection.Close();

        public void Open()
        {
            try
            {
                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();
            }
            catch (Exception ex)
            {
            }
        }
    }
}