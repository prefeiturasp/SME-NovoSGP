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
            //TODO: Adicionada verificação do estado de conexão para que não dê exception. Verificar depois a implementação correta e remover a verificação
            if (transacao != null && transacao.Connection != null && (transacao.Connection.State == ConnectionState.Fetching || transacao.Connection.State == ConnectionState.Executing))
            {
                transacao.Rollback();
            }
        }
    }
}