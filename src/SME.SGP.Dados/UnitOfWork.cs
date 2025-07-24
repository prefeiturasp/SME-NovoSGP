using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Data;

namespace SME.SGP.Dados
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ISgpContext sgpContext;
        private IDbTransaction transacao;
        private bool disposed = false;
        public bool TransacaoAberta { get; set; }

        public UnitOfWork(ISgpContext sgpContext)
        {
            this.sgpContext = sgpContext ?? throw new ArgumentNullException(nameof(sgpContext));
        }

        public IDbTransaction IniciarTransacao()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(UnitOfWork));

            // Verifica se já existe uma transação ativa
            if (transacao?.Connection?.State == ConnectionState.Open && TransacaoAberta)
            {
                return transacao;
            }

            // Limpa transação anterior se existir
            if (transacao != null)
            {
                try
                {
                    transacao.Dispose();
                }
                catch
                {
                    // Ignora erros ao fazer dispose de transação já finalizada
                }
                transacao = null;
            }

            try
            {
                // Garante que a conexão está aberta
                if (sgpContext.Conexao.State != ConnectionState.Open)
                {
                    sgpContext.AbrirConexao();
                }

                transacao = sgpContext.BeginTransaction();
                TransacaoAberta = true;

                return transacao;
            }
            catch (Exception)
            {
                TransacaoAberta = false;
                throw;
            }
        }

        public void PersistirTransacao()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(UnitOfWork));

            if (transacao != null && TransacaoAberta)
            {
                try
                {
                    // Verifica se a conexão ainda está válida
                    if (transacao.Connection?.State == ConnectionState.Open)
                    {
                        transacao.Commit();
                    }
                }
                catch (Exception)
                {
                    // Se falhar o commit, tenta rollback
                    try
                    {
                        if (transacao.Connection?.State == ConnectionState.Open)
                        {
                            transacao.Rollback();
                        }
                    }
                    catch
                    {
                        // Ignora erros no rollback
                    }
                    throw;
                }
                finally
                {
                    TransacaoAberta = false;
                    try
                    {
                        transacao?.Dispose();
                    }
                    catch
                    {
                        // Ignora erros no dispose
                    }
                    transacao = null;
                }
            }
        }

        public void Rollback()
        {
            if (disposed)
                return;

            if (transacao != null && TransacaoAberta)
            {
                try
                {
                    if (transacao.Connection?.State == ConnectionState.Open)
                    {
                        transacao.Rollback();
                    }
                }
                catch
                {
                    // Ignora erros no rollback
                }
                finally
                {
                    TransacaoAberta = false;
                    try
                    {
                        transacao?.Dispose();
                    }
                    catch
                    {
                        // Ignora erros no dispose
                    }
                    transacao = null;
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Faz rollback se ainda há transação ativa
                    if (TransacaoAberta)
                    {
                        Rollback();
                    }

                    // Dispose da transação se ainda existir
                    try
                    {
                        transacao?.Dispose();
                    }
                    catch
                    {
                        // Ignora erros no dispose
                    }
                    transacao = null;
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}
