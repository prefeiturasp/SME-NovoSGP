using System.Data;

namespace SME.SGP.Dominio
{
    public interface IUnitOfWork
    {
        IDbTransaction IniciarTransacao();

        void PersistirTransacao();

        void Rollback();
    }
}