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
        private readonly IDbConnection conexao; //Raphael - Trocado classe concreta de PgConnection pra IDbConnection
        private readonly IContextoAplicacao contextoAplicacao;

        public SgpContext(IConfiguration configuration, IContextoAplicacao contextoAplicacao, string stringConexao = "SGP_Postgres")
        {
            conexao = new NpgsqlConnection(configuration.GetConnectionString(stringConexao));
            this.contextoAplicacao = contextoAplicacao ?? throw new ArgumentNullException(nameof(contextoAplicacao));
            Open();
        }

        //Ctor para ser usado com o teste.
        public SgpContext(IDbConnection conexao, IContextoAplicacao contextoAplicacao)
        {
            this.conexao = conexao;
            this.contextoAplicacao = contextoAplicacao;
        }

        public IDbConnection Conexao
        {
            get
            {
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
        public string PerfilUsuario => contextoAplicacao.PerfilUsuario;

        public string UsuarioLogadoRF =>
                                          contextoAplicacao.ObterVariavel<string>("RF") ?? "0";

        public string Administrador => contextoAplicacao.Administrador;

        public IDbTransaction BeginTransaction()
        {
            if (conexao.State == ConnectionState.Closed)
                conexao.Open();

            return conexao.BeginTransaction();
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return conexao.BeginTransaction(il);
        }

        public void ChangeDatabase(string databaseName)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            conexao.Close();
        }

        public IDbCommand CreateCommand()
        {
            return conexao.CreateCommand();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (conexao.State == ConnectionState.Open)
                conexao.Close();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Open()
        {
            if (conexao.State != ConnectionState.Open)
                conexao.Open();
        }

        public void AbrirConexao()
        {
            Open();
        }

        public void FecharConexao()
        {
            if (conexao.State != ConnectionState.Closed)
            {
                Close();
            }
        }
    }
}
