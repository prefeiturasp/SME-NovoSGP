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
        private readonly IDbConnection conexao;
        private readonly IContextoAplicacao contextoAplicacao;
        private bool disposed = false;

        public SgpContext(IConfiguration configuration, IContextoAplicacao contextoAplicacao, string stringConexao = "SGP_Postgres")
        {
            var connectionString = configuration.GetConnectionString(stringConexao);

            conexao = new NpgsqlConnection(connectionString);
            this.contextoAplicacao = contextoAplicacao ?? throw new ArgumentNullException(nameof(contextoAplicacao));

            try
            {
                if (conexao.State != ConnectionState.Open)
                {
                    conexao.Open();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"ERRO CRÍTICO: Falha ao abrir a conexão com o banco de dados. Mensagem: {ex.Message}");
                throw new InvalidOperationException("Falha ao inicializar SgpContext: Não foi possível abrir a conexão com o banco de dados.", ex);
            }
        }

        // Construtor para testes
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

        public string ConnectionString
        {
            get { return Conexao.ConnectionString; }
            set { Conexao.ConnectionString = value; }
        }

        public int ConnectionTimeout => Conexao.ConnectionTimeout;

        public string Database => Conexao.Database;

        public ConnectionState State => Conexao.State;

        public string UsuarioLogado => contextoAplicacao.UsuarioLogado;

        public string UsuarioLogadoNomeCompleto => contextoAplicacao.NomeUsuario;

        public string PerfilUsuario => contextoAplicacao.PerfilUsuario;

        public string UsuarioLogadoRF => contextoAplicacao.ObterVariavel<string>("RF") ?? "0";

        public string Administrador => contextoAplicacao.Administrador;

        public IDbTransaction BeginTransaction()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(SgpContext));

            try
            {
                if (conexao.State != ConnectionState.Open)
                    conexao.Open();

                return conexao.BeginTransaction();
            }
            catch (Exception)
            {
                // Se falhar ao abrir conexão ou iniciar transação, tenta fechar a conexão
                try
                {
                    if (conexao.State == ConnectionState.Open)
                        conexao.Close();
                }
                catch
                {
                    // Ignora erros ao fechar
                }
                throw;
            }
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(SgpContext));

            try
            {
                if (conexao.State != ConnectionState.Open)
                    conexao.Open();

                return conexao.BeginTransaction(il);
            }
            catch (Exception)
            {
                try
                {
                    if (conexao.State == ConnectionState.Open)
                        conexao.Close();
                }
                catch
                {
                    // Ignora erros ao fechar
                }
                throw;
            }
        }

        public void ChangeDatabase(string databaseName)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            if (disposed)
                return;

            try
            {
                if (conexao.State != ConnectionState.Closed)
                    conexao.Close();
            }
            catch
            {
                // Ignora erros ao fechar
            }
        }

        public IDbCommand CreateCommand()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(SgpContext));

            return conexao.CreateCommand();
        }

        public void Open()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(SgpContext));

            if (conexao.State != ConnectionState.Open)
            {
                try
                {
                    conexao.Open();
                }
                catch (Exception)
                {
                    // Se falhar ao abrir, tenta limpar o pool de conexões
                    if (conexao is NpgsqlConnection npgsqlConn)
                    {
                        try
                        {
                            NpgsqlConnection.ClearPool(npgsqlConn);
                        }
                        catch
                        {
                            // Ignora erros ao limpar pool
                        }
                    }
                    throw;
                }
            }
        }

        public void AbrirConexao()
        {
            Open();
        }

        public void FecharConexao()
        {
            Close();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    try
                    {
                        if (conexao?.State == ConnectionState.Open)
                            conexao.Close();
                    }
                    catch
                    {
                        // Ignora erros ao fechar
                    }

                    try
                    {
                        conexao?.Dispose();
                    }
                    catch
                    {
                        // Ignora erros no dispose
                    }
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~SgpContext()
        {
            Dispose(false);
        }
    }
}
