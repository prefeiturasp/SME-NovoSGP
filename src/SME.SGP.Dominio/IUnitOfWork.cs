using System;
using System.Data;
using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public interface IUnitOfWork : IDisposable,IAsyncDisposable
    {
        IDbTransaction IniciarTransacao();
        void PersistirTransacao();
        void Rollback();

        Task<IDbTransaction> IniciarTransacaoAsync();
        Task PersistirTransacaoAsync();
        Task RollbackAsync();
        Task CommitAsync();
    }
}