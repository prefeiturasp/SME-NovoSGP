using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Data;

namespace SME.SGP.Dados.Contexto
{
    public class SgpContext : ISgpContext
    {
        private readonly NpgsqlConnection connection;
        private readonly IHttpContextAccessor httpContextAccessor;

        public SgpContext(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            connection = new NpgsqlConnection(configuration.GetConnectionString("SGP-Postgres"));
            Open();
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public string ConnectionString { get { return connection.ConnectionString; } set { connection.ConnectionString = value; } }

        public int ConnectionTimeout => connection.ConnectionTimeout;

        public string Database => connection.Database;

        public ConnectionState State => connection.State;

        public string UsuarioLogado =>
                                          httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Sistema";

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

        public NpgsqlConnection Conexao() => connection;

        public IDbCommand CreateCommand()
        {
            return connection.CreateCommand();
        }

        public void Dispose() => connection.Close();

        public void Open()
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
        }
    }
}