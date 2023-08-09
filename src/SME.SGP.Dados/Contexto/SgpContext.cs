using Microsoft.Extensions.Configuration;
using Npgsql;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Contexto
{
    public class SgpContext : ISgpContext
    {
        private readonly NpgsqlConnection conexao;
        private readonly IContextoAplicacao contextoAplicacao;

        public SgpContext(IConfiguration configuration, IContextoAplicacao contextoAplicacao,
            string stringConexao = "SGP_Postgres")
        {
            //o NpgsqlConnection tem suporte a async, dava pra tirar vantagem disso
            //ao trocar para interface deixa mais extensivel e testavel porem do jeito que esta hoje perde as vantagens
            //que a implementacao do NpgsqlConnection oferece.
            //Uma opcao seria complementar o ISGPContext com metodos async
            //e tirar proveito nos casos de chamada que suportem async/await
            var connectionString = configuration.GetConnectionString(stringConexao);
            this.conexao = new NpgsqlConnection(connectionString);
            this.contextoAplicacao = contextoAplicacao ?? throw new ArgumentNullException(nameof(contextoAplicacao));
            //Pelo profile boa parte do tempo esta sendo de espera de abertura de conexao e wait de thread
            //em escala abrindo desse jeito sync o throughput da aplicacao tende a diminuir
            //talvez mover a abertura para o comeco do request no filter
        }

        public SgpContext(IDbConnection conexao, IContextoAplicacao contextoAplicacao)
        {
            this.conexao = conexao as NpgsqlConnection;
            this.contextoAplicacao = contextoAplicacao;
        }

        public IDbConnection Conexao => conexao;

        public string ConnectionString
        {
            get => conexao.ConnectionString;
            set => conexao.ConnectionString = value;
        }

        public int ConnectionTimeout => conexao.ConnectionTimeout;

        public string Database => conexao.Database;

        public ConnectionState State => conexao.State;

        public string UsuarioLogadoNomeCompleto =>
            contextoAplicacao.NomeUsuario;

        public string PerfilUsuario => contextoAplicacao.PerfilUsuario;

        public string UsuarioLogadoRF =>
            contextoAplicacao.ObterVariavel<string>("RF") ?? "0";

        public string Administrador => contextoAplicacao.Administrador;

        public async Task OpenAsync()
        {
            if (IsNotOpened)
            {
                await conexao.OpenAsync();
            }
        }

        public async Task CloseAsync()
        {
            if (IsNotClosed)
            {
                await conexao.CloseAsync();
            }
        }

        public async Task<IDbTransaction> BeginTransactionAsync()
        {
            await OpenAsync();
            return await conexao.BeginTransactionAsync();
        }

        public IDbTransaction BeginTransaction()
        {
            Open();
            return conexao.BeginTransaction();
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return conexao.BeginTransaction(il);
        }

        public void ChangeDatabase(string databaseName)
        {
            conexao.ChangeDatabase(databaseName);
        }

        public void Open()
        {
            if (IsNotOpened)
            {
                conexao.Open();
            }
        }

        public void Close()
        {
            if (IsNotClosed)
            {
                conexao.Close();
            }
        }

        private bool IsNotOpened => conexao.State is not ConnectionState.Open;

        private bool IsNotClosed => conexao.State is not ConnectionState.Closed;

        public IDbCommand CreateCommand()
        {
            return conexao.CreateCommand();
        }

        public void Dispose()
        {
            Close();
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            await conexao.CloseAsync();
            await conexao.DisposeAsync();
        }
    }
}