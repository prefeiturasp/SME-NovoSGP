using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
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

        public IDbTransaction IniciarTransacao()
        {
            if (transacao == null)
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
            if (transacao != null)
            {
                transacao.Rollback();
            }
        }
    }
}
