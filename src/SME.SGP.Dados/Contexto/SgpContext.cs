using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Data;

namespace SME.SGP.Dados.Contexto
{
    public class SgpContext : ISgpContext
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public SgpContext(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            Conexao = new NpgsqlConnection(configuration.GetConnectionString("SGP-Postgres"));
            Open();
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public NpgsqlConnection Conexao { get; }
        public string ConnectionString { get { return Conexao.ConnectionString; } set { Conexao.ConnectionString = value; } }

        public int ConnectionTimeout => Conexao.ConnectionTimeout;

        public string Database => Conexao.Database;

        public ConnectionState State => Conexao.State;

        public string UsuarioLogado =>
                                       httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Sistema";

        public string UsuarioLogadoNomeCompleto =>
                                          httpContextAccessor.HttpContext?.User?.FindFirst("Nome")?.Value ?? "Sistema";

        public string UsuarioLogadoRF =>
                                          httpContextAccessor.HttpContext?.User?.FindFirst("RF")?.Value ?? "0";

        public IDbTransaction BeginTransaction()
        {
            return Conexao.BeginTransaction();
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return Conexao.BeginTransaction(il);
        }

        public void ChangeDatabase(string databaseName)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            Conexao.Close();
        }

        public IDbCommand CreateCommand()
        {
            return Conexao.CreateCommand();
        }

        public void Dispose() => Conexao.Close();

        public void Open()
        {
            if (Conexao.State != ConnectionState.Open)
                Conexao.Open();
        }
    }
}