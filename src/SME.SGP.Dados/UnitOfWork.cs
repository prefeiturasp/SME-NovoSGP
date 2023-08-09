using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    //O conceito de unit of work está implementado bem estranhamente ao meu ver,
    //No maximo essa implementacao externaliza o conceito de transacao e depende muito
    //do desenvolvedor gerenciar o ciclo de vida de abertura, commit e rollback,etc

    //Implementacoes mais comuns de UOW tendem a garantir que todos os repositorios compartilhem o mesmo contexto e que nao possam ser acessados
    //por outra forma de injecao a nao ser pelo proprio unit of work que vai garantir que todos os repos vao usar o mesmo contexto de conexao, transacao, etc

    //Eu aconselho a trabalhar com AOP usando dinamic proxies e controlando transacoes com custom attributes
    //anotando os metodos de comandos que precisa ser executados dentro de uma mesma transacao corrente

    //Outro caso mais facil de implementar é trabalhar com filtros no escopo de web IActionFilter, IAsyncActionFilter
    //que abrem uma transacao no comeco do request e ou faz commit ou rollback no fim do request dependendo se lancou ou nao exception

    //de todos os modos nao é aconselhavel depender de um desenvolvedor lembrar de gerenciar o ciclo de vida de uma transacao
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ISgpContext sgpContext;
        private DbTransaction dbTransaction;

        public UnitOfWork(ISgpContext sgpContext)
        {
            this.sgpContext = sgpContext ?? throw new System.ArgumentNullException(nameof(sgpContext));
        }


        //eu nao vazaria a transacao para o lado de fora do uow
        //ja que esta sendo usado como scoped o dispose vai ser chamado no final do request
        public IDbTransaction IniciarTransacao()
        {
            if (HasTransaction)
            {
                return dbTransaction;
            }

            dbTransaction = sgpContext.BeginTransaction() as DbTransaction;
            return dbTransaction;
        }

        public void PersistirTransacao()
        {
            if (HasTransaction)
            {
                dbTransaction.Commit();
                dbTransaction = null;
            }
        }

        public void Rollback()
        {
            if (HasTransaction)
            {
                dbTransaction.Rollback();
                dbTransaction = null;
            }
        }

        public async Task<IDbTransaction> IniciarTransacaoAsync()
        {
            if (HasTransaction)
            {
                return dbTransaction;
            }

            return await sgpContext.BeginTransactionAsync();
        }

        public async Task PersistirTransacaoAsync()
        {
            if (HasTransaction)
            {
                await dbTransaction.CommitAsync();
                dbTransaction = null;
            }
        }

        public async Task RollbackAsync()
        {
            if (HasTransaction)
            {
                await dbTransaction.RollbackAsync();
                dbTransaction = null;
            }
        }

        public async Task CommitAsync()
        {
            if (HasTransaction)
            {
                await dbTransaction.CommitAsync();
                dbTransaction = null;
            }
        }

        private bool HasTransaction => dbTransaction is not null;

        public void Dispose()
        {
            Rollback();
        }

        public async ValueTask DisposeAsync()
        {
            await RollbackAsync();
        }
    }
}