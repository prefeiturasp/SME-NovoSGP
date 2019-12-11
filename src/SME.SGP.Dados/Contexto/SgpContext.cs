using Microsoft.Extensions.Configuration;
using Npgsql;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Data;

namespace SME.SGP.Dados.Contexto
{
    public class SgpContext : ISgpContext
    {
        private readonly NpgsqlConnection conexao;
        private readonly IContextoAplicacao contextoAplicacao;

        public SgpContext(IConfiguration configuration, IContextoAplicacao contextoAplicacao)
        {
            conexao = new NpgsqlConnection(configuration.GetConnectionString("SGP-Postgres"));
            this.contextoAplicacao = contextoAplicacao ?? throw new ArgumentNullException(nameof(contextoAplicacao));
        }

        public IDbConnection Conexao
        {
            get
            {
                if (conexao.State != ConnectionState.Open)
                    Open();
                return conexao;
            }
        }

        public string ConnectionString { get { return Conexao.ConnectionString; } set { Conexao.ConnectionString = value; } }

        public int ConnectionTimeout => Conexao.ConnectionTimeout;

        public string Database => Conexao.Database;

        public ConnectionState State => Conexao.State;

        public string UsuarioLogado =>
                                       contextoAplicacao.UsuarioLogado;

        public string UsuarioLogadoNomeCompleto =>
                                          contextoAplicacao.NomeUsuario;

        public string UsuarioLogadoRF =>
                                          contextoAplicacao.ObterVarivel<string>("RF") ?? "0";


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
            if (conexao.State != ConnectionState.Open)
                conexao.Open();
        }
    }
}