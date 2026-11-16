using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Data;
using System.Threading;

namespace SME.SGP.Dados.Contexto
{
    public class SgpContextConsultasAbrangencia : ISgpContextConsultasAbrangencia
    {
        private readonly Lazy<ISgpContextConsultas> contexto;
        private readonly IContextoAplicacao contextoAplicacao;
        private bool disposed;

        public SgpContextConsultasAbrangencia(Func<ISgpContextConsultas> criarContexto, IContextoAplicacao contextoAplicacao)
        {
            if (criarContexto == null)
                throw new ArgumentNullException(nameof(criarContexto));

            this.contextoAplicacao = contextoAplicacao ?? throw new ArgumentNullException(nameof(contextoAplicacao));
            contexto = new Lazy<ISgpContextConsultas>(criarContexto, LazyThreadSafetyMode.ExecutionAndPublication);
        }

        public bool ConexaoCriada => contexto.IsValueCreated;

        private ISgpContextConsultas Contexto
        {
            get
            {
                if (disposed)
                    throw new ObjectDisposedException(nameof(SgpContextConsultasAbrangencia));

                return contexto.Value;
            }
        }

        public IDbConnection Conexao => Contexto.Conexao;

        public string UsuarioLogado => contextoAplicacao.UsuarioLogado;

        public string PerfilUsuario => contextoAplicacao.PerfilUsuario;

        public string UsuarioLogadoNomeCompleto => contextoAplicacao.NomeUsuario;

        public string UsuarioLogadoRF => contextoAplicacao.ObterVariavel<string>("RF") ?? "0";

        public string Administrador => contextoAplicacao.Administrador;

        public string ConnectionString
        {
            get => Contexto.ConnectionString;
            set => Contexto.ConnectionString = value;
        }

        public int ConnectionTimeout => Contexto.ConnectionTimeout;

        public string Database => Contexto.Database;

        public ConnectionState State => ConexaoCriada && !disposed ? contexto.Value.State : ConnectionState.Closed;

        public void AbrirConexao() => Contexto.AbrirConexao();

        public void FecharConexao() => Close();

        public IDbTransaction BeginTransaction() => Contexto.BeginTransaction();

        public IDbTransaction BeginTransaction(IsolationLevel il) => Contexto.BeginTransaction(il);

        public void ChangeDatabase(string databaseName) => Contexto.ChangeDatabase(databaseName);

        public void Close()
        {
            if (ConexaoCriada && !disposed)
                contexto.Value.Close();
        }

        public IDbCommand CreateCommand() => Contexto.CreateCommand();

        public void Open() => Contexto.Open();

        public void Dispose()
        {
            if (disposed)
                return;

            if (ConexaoCriada)
                contexto.Value.Dispose();

            disposed = true;
            GC.SuppressFinalize(this);
        }
    }
}
