using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Data;

namespace SME.SGP.Dados
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ISgpContext sgpContext;
        private IDbTransaction transacao;

        public UnitOfWork(ISgpContext sgpContext)
        {
            this.sgpContext = sgpContext ?? throw new System.ArgumentNullException(nameof(sgpContext));
        }

        public void Dispose()
        {
            Rollback();
        }

        public IDbTransaction IniciarTransacao()
        {
            if (transacao == null || transacao?.Connection?.State == null)
                transacao = sgpContext.BeginTransaction();

            return transacao;
        }

        public void PersistirTransacao()
        {
            if (transacao != null)
            {
                transacao.Commit();
            }
        }

        public void Rollback()
        {
            if (transacao != null && transacao.Connection != null)
            {
                transacao.Rollback();
            }
        }
    }
}