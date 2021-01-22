using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Data;

namespace SME.SGP.Dados
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ISgpContext sgpContext;
        private IDbTransaction transacao;
        public bool TransacaoAberta { get; set; }

        public UnitOfWork(ISgpContext sgpContext)
        {
            this.sgpContext = sgpContext ?? throw new System.ArgumentNullException(nameof(sgpContext));
        }

        public void Dispose()
        {
            if (TransacaoAberta)
                Rollback();
        }

        public IDbTransaction IniciarTransacao()
        {
            if (transacao == null || transacao?.Connection?.State == null && !TransacaoAberta)
            {
                transacao = sgpContext.BeginTransaction();
                TransacaoAberta = true;
            }
                
            return transacao;
        }

        public void PersistirTransacao()
        {
            if (transacao != null && TransacaoAberta)
            {
                transacao.Commit();
                TransacaoAberta = false;
                transacao = null;
            }
        }

        public void Rollback()
        {
            if (transacao != null && transacao.Connection != null && TransacaoAberta)
            {
                transacao.Rollback();
                TransacaoAberta = false;
            }
        }
    }
}